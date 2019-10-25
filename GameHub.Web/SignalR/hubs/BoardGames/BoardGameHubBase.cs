using System;
using System.Threading.Tasks;
using Caching;
using GameHub.Games.BoardGames.Common;
using Microsoft.AspNetCore.SignalR;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public abstract class BoardGameHubBase<T> : Hub where T : IBoardGame<GameState, GamePlayer>
    {
        protected ICache<T> _cache;

        public BoardGameHubBase(ICache<T> cache)
        {
            _cache = cache;
        }

        public virtual void StartGame(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return;
            }

            lock(game)
            {
                var playerId = Context.Items["PlayerId"].ToString();

                var startResult = game.StartGame(playerId);

                if (startResult.WasSuccessful)
                {
                    Clients.Group(gameId).SendAsync("GameStarted", game.GetGameState());
                }
                else
                {
                    Clients.Caller.SendAsync("IllegalAction", "Couldn't start game");
                }
            }
        }

        // currently this is both a hub endpoint and used by endpoints themselves to get the game state. 
        // todo: make them separate, may not want to send roomdoesntexist.
        public GameState GetGameState(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return null;
            }
            
            return game.GetGameState();
        }

        public GamePlayer GetClientPlayerInfo(string gameId)
        {    
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return null;
            }

            return game.GetPlayer(playerId);
        }

        public virtual void JoinRoom(string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return;
            }

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public virtual void JoinGame(string gameId, string playerNick)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var gamestate = game.GetGameState();  

            var registerResult = new ActionResult(false); //default

            // todo: dont do this check here, check in the game and return a value indicating success.
            if (gamestate.Status.Status == GameHub.Games.BoardGames.Common.GameStatus.lobby.ToString())
            {
                registerResult = game.RegisterPlayer(playerId, playerNick);
            }

            if (registerResult.WasSuccessful)
            {
                Clients.Group(gameId).SendAsync("PlayerJoined", this.GetGameState(gameId));
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", registerResult.Message);
            }
        }

        public virtual void Rematch(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            if (game == null) return;
            
            lock(game)
            {
                var resetResult = game.Reset(playerId);

                if (resetResult.WasSuccessful)
                {
                    Clients.Group(gameId).SendAsync("GameStarted", this.GetGameState(gameId));
                }
                else
                {
                    Clients.Caller.SendAsync("IllegalAction", resetResult.Message);
                }
            }
        }


        public virtual void Resign(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            lock(game)
            {
                game.Resign(playerId);

                Clients.Group(gameId).SendAsync("PlayerResigned", GetGameState(gameId));
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