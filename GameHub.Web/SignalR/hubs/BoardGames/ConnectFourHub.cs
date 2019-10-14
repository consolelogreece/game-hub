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
    public class ConnectFourHub : BoardGameHubBase<ConnectFour>
    {
        public ConnectFourHub([FromServices] ICache<ConnectFour> cache) : base(cache)
        {}

        public ConnectFourPlayer GetClientPlayerInfo(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return null;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            return game.GetPlayer(playerId);      
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
    }
}
