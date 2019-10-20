using System;
using System.Collections.Generic;
using System.Text;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class GameStateConnectFour : GameState
    {
        public List<ConnectFourPlayer> Players { get; set; }

        public string[][] BoardState { get; set; }

        public ConnectFourPlayer CurrentTurnPlayer { get; set; }

        public ConnectFourConfiguration Configuration { get; set; }
    }
}
