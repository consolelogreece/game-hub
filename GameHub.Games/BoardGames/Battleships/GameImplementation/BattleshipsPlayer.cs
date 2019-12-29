using System.Collections.Generic;
using System.Linq;


namespace GameHub.Games.BoardGames.Battleships
{
    public class Player
    {
        private Board _board;

        private List<Ship> _ships;

        public Player(Board board)
        {
            _board = board;
        }

        public void RegisterShips(List<Ship> ships)
        {
            ships.ForEach(s => _board.MapShip(s));
        }

        public bool IsGameOver()
        {
            return _ships.All(s => s.IsSunk());
        }

        private bool SquareAlreadyHit(int row, int col)
        {
            return _board[row,col].hit;
        }

        public BattleshipsMoveResult RegisterHit(BattleshipsMove move)
        {
            if (SquareAlreadyHit(move.row, move.col)) return new BattleshipsMoveResult(false);

            _board[move.row, move.col].hit = true;

            if (_board[move.row, move.col].occupyingShip != null) _board[move.row, move.col].occupyingShip.hit();

            return new BattleshipsMoveResult(true, "", _board[move.row, move.col].occupyingShip, IsGameOver());
        }
    }
}