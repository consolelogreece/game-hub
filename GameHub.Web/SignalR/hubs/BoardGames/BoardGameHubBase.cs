using System;
using System.Threading.Tasks;
using Caching;
using GameHub.Games.BoardGames.Common;
using Microsoft.AspNetCore.SignalR;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public abstract class BoardGameHubBase<T> : Hub where T : IBoardGame
    {
        protected ICache<T> _cache;
        public BoardGameHubBase(ICache<T> cache)
        {
            _cache = cache;
        }

        public virtual void StartGame(string gameId)
        {
            var game = _cache.Get(gameId);

            var started = game.StartGame();

            if (started)
            {
                Clients.Group(gameId).SendAsync("GameStarted", game.GetGameState());
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", "Couldn't start game");
            }
        }

        public GameStateBase GetGameState(string gameId)
        {
            return _cache.Get(gameId).GetGameState();
        }

        public virtual void JoinRoom(string gameId)
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

            if (gamestate.Status.Status == GameHub.Games.BoardGames.Common.GameStatus.lobby.ToString())
            {
                registeredSuccessfully = game.RegisterPlayer(playerId, playerNick);
            }

            return registeredSuccessfully;
        }

        public virtual void Rematch(string gameId)
        {
            var game = _cache.Get(gameId);

            var playerId = Context.Items["PlayerId"].ToString();

            if (game == null) return;

            var resetSuccessful = game.Reset(playerId);

            if (resetSuccessful)
            {
                Clients.Group(gameId).SendAsync("GameStarted", this.GetGameState(gameId));
            }
        }

        public virtual void Resign(string gameId)
        {
            var game = _cache.Get(gameId);

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