using System.Collections.Generic;
using System.Linq;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsGame
    {
        public Player p1 {get; private set;}
        public Player p2 {get; private set;}

        public Player Winner {
            get {
                if (p1.IsGameOver()) return p2;
                if (p2.IsGameOver()) return p1;
                return null;
            }
        }

        private int nextTurnPlayer = 1;
        private int _rows;

        private int _cols;
        public BattleshipsGame(int rows, int cols)
        {
            _rows = rows;
            
            _cols = cols;
        }

        public void Register(List<ShipModel> shipModels, string playerId, PlayerNumber playerNumber)
        {
            if (p1 != null && p2 != null) throw new System.Exception("wat");

            var board = new Board(_rows, _cols);

            var newPlayer = new Player(board, playerId);

            var ships = shipModels.Select(sm => new Ship(sm)).ToList();

            newPlayer.RegisterShips(ships);

            if (playerNumber == PlayerNumber.One) p1 = newPlayer;
            else p2 = newPlayer;
        }

        public BattleshipsMoveResult Move(BattleshipsPosition move)
        {
            var defendingPlayer = nextTurnPlayer == -1 ? p1 : p2;
            
            var result = defendingPlayer.RegisterHit(move);

            if (result.WasSuccessful)
            {
                nextTurnPlayer *= -1;
            }

            return result;
        }
    }
}