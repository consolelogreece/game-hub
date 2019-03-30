using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        IConnectFour _game;

        public ConnectFourHub(IConnectFour game)
        {
            _game = game;
        }

        public void MakeMove(int col, string player)
        {
            var result = _game.MakeMove(col, player);

            if (result.WasValidMove)
            {
                Clients.All.SendAsync("PlayerMoved", result);
            }
        }
    }
}
