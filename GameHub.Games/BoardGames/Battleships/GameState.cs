using System.Collections.Generic;
using GameHub.Games.BoardGames.Battleships;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsGameState : GameState
    {
        public BattleshipsPlayer CurrentTurnPlayer {get;set;}

        public List<BattleshipsPlayer> Players { get; set; }

        public BattleshipsConfiguration Configuration { get; set; }
    }
}