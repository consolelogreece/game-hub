using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.Chess
{
    public class MoveResult
    {
        public ChessPlayer Player { get; internal set; }

        public string Fen { get; set; }

        public bool WasValidMove { get; internal set; }

        public string Message { get; set; }

        public bool DidMoveWin { get; internal set; }

        public ChessPlayer NextTurnPlayer { get; internal set; }
    }
}
