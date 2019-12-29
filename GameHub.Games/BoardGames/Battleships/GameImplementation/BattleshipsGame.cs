using System.Collections.Generic;
using System.Linq;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsGame
    {
        private Player p1;
        private Player p2;

        private int nextTurnPlayer = 1;

        private BattleshipsConfiguration _config;
        public BattleshipsGame(BattleshipsConfiguration config)
        {
            _config = config;
        }

        private void initializePlayer(Player player)
        {
            var board = new Board(_config.rows, _config.cols);

            player = new Player(board);
        }

        private void Register(List<ShipModel> shipModels)
        {
            if (p1 != null && p2 != null) throw new System.Exception("wat");

            var player = p1 == null ? p1 : p2;

            initializePlayer(player);

            var ships = shipModels.Select(sm => new Ship(sm)).ToList();

            player.RegisterShips(ships);
        }

        public BattleshipsMoveResult Move(BattleshipsMove move)
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