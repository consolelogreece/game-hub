using System.Collections.Generic;
using System.Linq;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsGame
    {
        public Player p1 {get; private set;}
        public Player p2 {get; private set;}

        private int nextTurnPlayer = 1;

        private BattleshipsConfiguration _config;
        public BattleshipsGame(BattleshipsConfiguration config)
        {
            _config = config;
        }

        public void Register(List<ShipModel> shipModels, string playerId)
        {
            if (p1 != null && p2 != null) throw new System.Exception("wat");

            var board = new Board(_config.rows, _config.cols);

            var newPlayer = new Player(board, playerId);

            var ships = shipModels.Select(sm => new Ship(sm)).ToList();

            newPlayer.RegisterShips(ships);

            if (p1 == null) p1 = newPlayer;
            else p2 = newPlayer;
        }

        // public void RegisterShips(List<ShipModel> shipModels, st)
        // {
        //     var ships = shipModels.Select(sm => new Ship(sm)).ToList();

        //     var player = p1 == null ? p1 : p2;

        //     player.RegisterShips(ships);
        // }

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