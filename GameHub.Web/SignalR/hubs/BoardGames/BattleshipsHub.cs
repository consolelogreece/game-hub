using Microsoft.AspNetCore.SignalR;
using System;
using GameHub.Web.Services.Games.ConnectFourServices;
using System.Threading.Tasks;
using GameHub.Games.BoardGames.Common;
using GameHub.Web.Models;
using GameHub.Games.BoardGames.Battleships;
using System.Collections.Generic;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class BattleshipsHub : Hub
    {
        private IBattleshipsServiceFactory _battleshipsServiceFactory;
        public BattleshipsHub(IBattleshipsServiceFactory battleshipsServiceFactory)
        {
            _battleshipsServiceFactory = battleshipsServiceFactory;
        }

        public BattleshipsGameState GetGameState()
        {
           return GetGameService().GetGameState();
        }

        private BattleshipsService GetGameService()
        {
            return Context.Items["GameService"] as BattleshipsService;
        }

        private void InformClients(string endpoint)
        {
            var gameId = Context.Items["GameId"].ToString();

            Clients.Group(gameId).SendAsync(endpoint);
        }

        public ActionResult StartGame() 
        {
            var result = GetGameService().StartGame();

            if (result.WasSuccessful)
            {
                InformClients("GameStarted");
            }

            return result;
        }

        public ActionResult JoinGame()
        {
            var username = GetUserRequestMeta().profile.Username;

            var result = GetGameService().JoinGame(username);

            if (result.WasSuccessful)
            {
                InformClients("PlayerJoined");
            }

            return result;
        }

        private UserRequestMeta GetUserRequestMeta()
        {
            return Context.Items["user"] as UserRequestMeta;
        }

        public BattleshipsPlayerModel GetClientPlayerInfo()
        {
            return GetGameService().GetPlayer();
        }

        public ActionResult Resign() 
        {
            var result = GetGameService().Resign();

            if (result.WasSuccessful)
            {
                InformClients("PlayerResigned");
            }

            return result;
        }

        public ActionResult Rematch() 
        {
            var result = GetGameService().Restart();

            if (result.WasSuccessful)
            {
                InformClients("RematchStarted");
            }

            return result;
        }

        public ActionResult Move(BattleshipsPosition move) 
        {
            var result = GetGameService().Move(move);

            if (result.WasSuccessful)
            {
                InformClients("PlayerMoved");
            }

            return result;
        }

        public ActionResult RegisterShips(List<ShipModel> shipModels) 
        {
            return GetGameService().RegisterShips(shipModels);
        }

        public override Task OnConnectedAsync()
        {
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

            var service = _battleshipsServiceFactory.Create(gameId, player.profile.Id);

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