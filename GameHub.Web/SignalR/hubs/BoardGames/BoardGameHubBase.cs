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

            var playerId = Context.Items["PlayerId"].ToString();

            var started = game.StartGame(playerId);

            if (started)
            {
                Clients.Group(gameId).SendAsync("GameStarted", game.GetGameState());
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", "Couldn't start game");
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

        public virtual bool JoinGame(string gameId, string playerNick)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return false;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var gamestate = game.GetGameState();

            var registeredSuccessfully = false;

            // todo: dont do this check here, check in the game and return a value indicating success.
            if (gamestate.Status.Status == GameHub.Games.BoardGames.Common.GameStatus.lobby.ToString())
            {
                registeredSuccessfully = game.RegisterPlayer(playerId, playerNick);
            }

            if (registeredSuccessfully)
            {
                Clients.Group(gameId).SendAsync("PlayerJoined", this.GetGameState(gameId));
            }

            return registeredSuccessfully;
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

            var resetSuccessful = game.Reset(playerId);

            if (resetSuccessful)
            {
                Clients.Group(gameId).SendAsync("GameStarted", this.GetGameState(gameId));
            }
        }

        // todo: have a player resigned method on front end hub. resignations dont necessarily mean the game is over.
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

            game.Resign(playerId);

            Clients.Group(gameId).SendAsync("GameOver", GetGameState(gameId));
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