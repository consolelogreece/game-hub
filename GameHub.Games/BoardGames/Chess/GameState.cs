using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Chess
{
    public class GameStateChess : GameState
    {
        public string BoardStateFen { get; set; }

        public ChessPlayer CurrentTurnPlayer {get;set;}

        public List<ChessPlayer> Players { get; set; }

        public ChessConfig Configuration { get; set; }
    }
}