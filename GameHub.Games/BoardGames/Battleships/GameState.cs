using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsGameState : GameState
    {
        public BattleshipsPlayerModel CurrentTurnPlayer {get;set;}

        public List<BattleshipsPlayerModel> Players { get; set; }
        
        public BattleshipsConfiguration Configuration { get; set; }

        // shows where enemy has shot
        public Square[,] PlayerBoard {get; set;}
        
        public List<Ship> PlayerShips {get; set;}

        // shows where player has shot
        public Square[,] OpponentBoard {get; set;}
    }
}