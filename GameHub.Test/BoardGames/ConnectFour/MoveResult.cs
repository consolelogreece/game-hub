using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class MoveResult
    {
        public string player { get; internal set; }

        public int row { get; internal set; }

        public int col { get; internal set; }

        public bool WasValidMove { get; internal set; }

        public string Message { get; set; }

        public bool DidMoveWin { get; internal set; }
    }
}
