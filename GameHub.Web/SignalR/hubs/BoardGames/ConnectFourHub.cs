using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        //TODO: Manually purge completed or inactive games to prevent mem leak. maybe extract this functional into own service then inject that 
        static ConcurrentDictionary<string, IConnectFour> _games = new ConcurrentDictionary<string, IConnectFour>();

        public void MakeMove(string gameId, int col, string player)
        {
            var result = _games[gameId].MakeMove(col, player);

            if (result.WasValidMove)
            {
                Clients.Group(gameId).SendAsync("PlayerMoved", result);
            }
            else
            {
                // TODO: Dedicated invalid move endpoint on front end
                Clients.Caller.SendAsync("PlayerMoved", result);
            }
        }

        public void CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            var createdSuccessfully = _games.TryAdd(Id, new ConnectFour());

            if (createdSuccessfully)
            {
                Clients.Caller.SendAsync("RoomCreatedRedirect", Id);
            }
        }

        public void JoinRoom(Guid gameId)
        {
            // ensure game exists before adding to group
            Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}
