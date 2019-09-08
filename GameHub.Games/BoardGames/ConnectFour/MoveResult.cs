using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class MoveResult
    {
        public string PlayerNick { get; internal set; }

        public string PlayerColor { get; internal set; }

        public string[][] BoardState { get; internal set; }

        public bool WasValidMove { get; internal set; }

        public string Message { get; set; }

        public bool DidMoveWin { get; internal set; }

        public string NextTurnPlayer { get; internal set; }
    }
}
