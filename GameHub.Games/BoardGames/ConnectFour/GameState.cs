using System;
using System.Collections.Generic;
using System.Text;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    // TODO: This needs to be reworked. game state should contain only the state of the game. isgamecreator and registerednick can be worked out later. maybe even remove everything excet board state and have another method for getting meta.
    public class GameStateConnectFour : GameState
    {
        public List<ConnectFourPlayer> Players { get; set; }

        public string[][] BoardState { get; set; }

        public ConnectFourPlayer CurrentTurnPlayer { get; set; }

        public ConnectFourConfiguration Configuration { get; set; }
    }
}
