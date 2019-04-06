﻿using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        //TODO: Manually purge completed or inactive games to prevent mem leak. maybe extract this functional into own service then inject that 
        // TODO: Find better way to transmit errors to user, throwing exceptions is expensive.
        static ConcurrentDictionary<string, IConnectFour> _games = new ConcurrentDictionary<string, IConnectFour>();

        public void StartGame(string gameId)
        {
            // TODO: Don't allow game to start without 2 players.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            var startedSuccessfully = game.Start(playerId);

            if (startedSuccessfully)
            {
                Clients.Group(gameId).SendAsync("GameStarted");
            }
            else
            {
                throw new HubException("You are not the game creator");
            }     
        }

        public void MakeMove(string gameId, int col)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _games[gameId];

            var result = game.MakeMove(col, playerId);

            if (result.WasValidMove)
            {
                Clients.Group(gameId).SendAsync("PlayerMoved", result);

                if (result.DidMoveWin)
                {
                    Clients.Group(gameId).SendAsync("PlayerWon", result.PlayerNick);
                }        
            }
            else
            {
                // TODO: Dedicated invalid move endpoint on front end
                Clients.Caller.SendAsync("InvalidMove", result);
            }
        }

        public string CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            if (!config.Validate()) throw new HubException("Invalid game config");

            var createdSuccessfully = _games.TryAdd(Id, new ConnectFour(config));

            if (createdSuccessfully)
            {
                return Id;
            }
            else
            {
                throw new HubException("Failed to create room");
            }
        }

        public void JoinRoom(string gameId, string playerNick)
        {
            // todo: stop breaking error on front end when player attempts to join but already has. perhaps make a check on componentdidmount and remove join option if already registered too.
            // todo: check to make sure game exists first.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            var registerResult = game.RegisterPlayer(playerId, playerNick);

            if (registerResult.Successful)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            }

            Clients.Caller.SendAsync("RoomJoined", registerResult);  
        }

        public override Task OnConnectedAsync()
        {
            // Get player id from http context. This is taken from a cookie and put in httpcontext items dictionary in an earlier piece of middleware.
            var httpContext = Context.GetHttpContext();

            if (httpContext.Items.ContainsKey("GHPID") == false)
            {
                throw new Exception("Got to hub without GHPID. This shouldn't happen, everybody panic!");
            }

            var ghpid = httpContext.Items["GHPID"].ToString();

            // Store playerid in hub context.
            Context.Items.Add("PlayerId", ghpid);

            return base.OnConnectedAsync();
        }
    }
}
