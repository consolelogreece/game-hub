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

        public MoveConsequence RegisterHit(BattleshipsPosition move)
        {
            return Board.Hit(move);  
        }
    }
}