using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class GameState
    {
        public string Status { get; set; }

        public bool IsPlayerRegistered { get; set; }

        public string RegisteredNick { get; set; }

        public bool IsGameCreator { get; set; }

        public string[][] BoardState { get; set; }
    }
}
