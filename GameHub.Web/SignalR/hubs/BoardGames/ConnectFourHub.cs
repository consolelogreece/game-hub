using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.AspNetCore.Mvc;
using Caching;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : BoardGameHubBase<ConnectFour>
    {
        public ConnectFourHub([FromServices] ICache<ConnectFour> cache) : base(cache)
        {}

        public void MakeMove(string gameId, int col)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return;
            }

            lock(game)
            {
                var playerId = Context.Items["PlayerId"].ToString();

                var moveResult = game.MakeMove(col, playerId);

                if (moveResult.WasSuccessful)
                {
                    Clients.Group(gameId).SendAsync("PlayerMoved", this.GetGameState(gameId));      
                }
                else
                {
                    Clients.Caller.SendAsync("IllegalAction", moveResult.Message);
                } 
            }
        }
        
        public string CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            var playerId = Context.Items["PlayerId"].ToString();

            config.creatorId = playerId;

            var errors = config.Validate();

            if (errors.Count != 0)
            {
               Clients.Caller.SendAsync("IllegalConfiguration", errors);

               return null;
            }

            var game = new ConnectFour(config);

            _cache.Set(Id, game);

            return Id;
        }
    }
}
