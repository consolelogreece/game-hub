using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class RegisterResult
    {
        // join type, e.g. spectator, player...
        public string JoinType { get; set; }

        public string[][] BoardState { get; set; }

        public bool IsFirstJoin { get; set; }

        public bool Successful { get; set; }
    }
}
