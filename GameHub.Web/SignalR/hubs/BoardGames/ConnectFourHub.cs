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

        public void StartGame(string gameId)
        {
            var game = _games[gameId];

            var firstplayer = game.Start();

            Clients.Group(gameId).SendAsync("GameStarted", firstplayer.PlayerNick);
        }

        public void MakeMove(string gameId, int col)
        {
            // TODO: Find way to persist connection ID or use a different way of ID
            var result = _games[gameId].MakeMove(col, Context.ConnectionId);

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

        public void JoinRoom(string gameId, string playerNick, string playerColor)
        {
            // check to make sure game exists first.
            var game = _games[gameId];

            var player = new ConnectFourPlayer { Id = Context.ConnectionId, PlayerNick = playerNick, PlayerColor = playerColor };

            var joinedSuccessfully = game.RegisterPlayer(player);

            if (Clients.Group(Context.ConnectionId) != null)
            {
                if (joinedSuccessfully)
                {
                    Clients.Caller.SendAsync("RoomJoinedPlayer");
                }
                else
                {
                    Clients.Caller.SendAsync("RoomJoinedSpectator");
                }

                Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            }
        }
    }
}
