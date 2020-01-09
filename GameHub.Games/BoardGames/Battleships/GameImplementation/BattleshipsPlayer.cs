using System.Collections.Generic;
using System.Linq;

namespace GameHub.Games.BoardGames.Battleships
{
    public class Player
    {
        public Board Board {get; private set;}

        public readonly string PlayerId;

        public List<Ship> Ships {get; private set;}

        public Player(Board board, string playerId)
        {
            Board = board;

            PlayerId = playerId;
        }

        public void RegisterShips(List<Ship> ships)
        {
            Ships = ships;
            ships.ForEach(s => Board.MapShip(s));
        }

        public bool IsGameOver()
        {
            return Ships.All(s => s.IsSunk());
        }        

        public BattleshipsMoveResult RegisterHit(BattleshipsPosition move)
        {
            var validMove = Board.Hit(move);

            if (!validMove) return new BattleshipsMoveResult(false);

            return new BattleshipsMoveResult(true, "", IsGameOver());
        }
    }
}