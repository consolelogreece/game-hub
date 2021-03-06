﻿using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using GameHub.Web.Services.Games.ConnectFourServices;
using System.Threading.Tasks;
using GameHub.Games.BoardGames.Common;
using GameHub.Web.Models;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        private IConnectFourServiceFactory _connectFourServiceFactory;
        public ConnectFourHub(IConnectFourServiceFactory connectFourServiceFactory)
        {
            _connectFourServiceFactory = connectFourServiceFactory;
        }

        private ConnectFourService GetGameService()
        {
            return Context.Items["GameService"] as ConnectFourService;
        }

        public ConnectFourGameState GetGameState()
        {
            return GetGameService().GetGameState();
        }

        private void SendGamestate(string endpoint)
        {
            var gameState = GetGameState();

            var gameId = Context.Items["GameId"].ToString();

            Clients.Group(gameId).SendAsync(endpoint, gameState);
        }

        public ActionResult StartGame() 
        {
            var result = GetGameService().StartGame();

            if (result.WasSuccessful)
            {
                SendGamestate("GameStarted");
            }

            return result;
        }

        public ActionResult JoinGame()
        {
            var username = GetUserRequestMeta().profile.Username;

            var result = GetGameService().JoinGame(username);
            
            if (result.WasSuccessful)
            {
                SendGamestate("PlayerJoined");
            }
            
            return result;
        }

        public UserRequestMeta GetUserRequestMeta()
        {
            return Context.Items["user"] as UserRequestMeta;
        }

        public ConnectFourPlayer GetClientPlayerInfo()
        {
            return GetGameService().GetPlayer();
        }

        public ActionResult Resign() 
        {
            var result = GetGameService().Resign();

            if (result.WasSuccessful)
            {
                SendGamestate("PlayerResigned");
            }
            
            return result;
        }

        public ActionResult Rematch() 
        {
            var result = GetGameService().Restart();

            if (result.WasSuccessful)
            {
                SendGamestate("RematchStarted");
            }
            
            return result;
        }

        public ActionResult Move(int col) 
        {
            var result = GetGameService().Move(col);

            if (result.WasSuccessful)
            {
                SendGamestate("PlayerMoved");
            }
            
            return result;
        }

        public override Task OnConnectedAsync()
        {
            // Get player id from http context. This is taken from a cookie and put in httpcontext items dictionary in an earlier piece of middleware.
            var httpContext = Context.GetHttpContext();

            if (!httpContext.Items.ContainsKey("user"))
            {
                throw new Exception("Got to hub without user. This shouldn't happen, everybody panic!");
            }

            if (!httpContext.Request.Query.ContainsKey("g"))
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Game doesn't exist"));
            }

            var player = httpContext.Items["user"] as UserRequestMeta;

            if (!player.isSignedIn)
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Not signed in"));
            }

            var gameId = httpContext.Request.Query["g"];

            var service = _connectFourServiceFactory.Create(gameId, player.profile.Id);

            if (service == null)
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Game doesn't exist"));
            }

            // Have to store in context instead of properties as otherwise can't access them in later calls.
            Context.Items.Add("GameService", service);

            // Store playerid in hub context.
            Context.Items.Add("user", player);

            // Store gameid in hub context
            Context.Items.Add("GameId", gameId);

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            return base.OnConnectedAsync();
        }
    }
}