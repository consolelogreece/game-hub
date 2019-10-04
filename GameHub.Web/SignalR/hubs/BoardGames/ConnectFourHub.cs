using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Caching;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        // todo: register signalr connection on reconnect 

        private ConnectFourCache _cache; 
 
        public ConnectFourHub([FromServices] ConnectFourCache cache)
        {
            _cache = cache;
        }

        public void StartGame(string gameId)
        {
            var game = _cache.Get(gameId);

            var playerId = Context.Items["PlayerId"].ToString();

            if (game.GetGameState().Players.Count < 2) 
            {
                Clients.Caller.SendAsync("IllegalAction", "Not enough players to start");
            }
            else
            {
                var startedSuccessfully = game.Start(playerId);

                if (startedSuccessfully)
                {
                    Clients.Group(gameId).SendAsync("GameStarted", this.GetGameState(gameId));
                }
                else
                {
                    Clients.Caller.SendAsync("IllegalAction", "You are not the host");
                }     
            }
        }

        public GameState GetGameState(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return null;
            }

            return game.GetGameState();
        }

        public ConnectFourPlayer GetClientPlayerInfo(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return null;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var state = game.GetGameState();

            return state.Players.FirstOrDefault(p => p.Id == playerId);            
        }

        public void MakeMove(string gameId, int col)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var result = game.MakeMove(col, playerId);

            if (result.WasValidMove)
            {
                Clients.Group(gameId).SendAsync("PlayerMoved", result);

                if (result.DidMoveWin)
                {
                    Clients.Group(gameId).SendAsync("PlayerWon", result.Player);
                }        
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", result.Message);
            }
        }

        public string CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            var playerId = Context.Items["PlayerId"].ToString();

            config.creatorId = playerId;

            var validGameConfig = config.Validate();

            if (!validGameConfig)
            {
               Clients.Caller.SendAsync("Config illegal");

               return null;
            }

            var game = new ConnectFour(config);

            _cache.Set(Id, game);

            return Id;
        }

        public void JoinRoom(string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return;
            }

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public void JoinGame(string gameId, string playerNick)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var gamestate = game.GetGameState();  

            var registeredSuccessfully = false;

            if (gamestate.Status == GameStatus.lobby.ToString())
            {
                registeredSuccessfully = game.RegisterPlayer(playerId, playerNick);
            }
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
