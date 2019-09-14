using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class MoveResult
    {
        public ConnectFourPlayer Player { get; internal set; }

        public string[][] BoardState { get; internal set; }

        public bool WasValidMove { get; internal set; }

        public string Message { get; set; }

        public bool DidMoveWin { get; internal set; }

        public ConnectFourPlayer NextTurnPlayer { get; internal set; }
    }
}
