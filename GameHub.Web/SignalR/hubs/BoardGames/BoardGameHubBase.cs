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