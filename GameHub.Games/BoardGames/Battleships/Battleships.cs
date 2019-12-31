using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class Battleships : IJoinable, 
        IRestartable, 
        IStartable, 
        IResignable, 
        IMoveable<BattleshipsPosition>,
        IGamePlayerGetter<BattleshipsPlayerModel>
    {
        #region private props
        private BattleshipsGame _game;

        private BattleshipsConfiguration _config;

        private BattleshipsPlayerModel p1;

        private BattleshipsPlayerModel p2;

        private int _nextTurnPlayerIndex = 1;

        private bool _started;

        #endregion

        public Battleships(BattleshipsConfiguration config)
        {
            _config = config;

            _game = new BattleshipsGame(config);
        }

        public BattleshipsGameState GetGameState()
        {
            throw new System.NotImplementedException();
        }

        public BattleshipsPlayerModel GetPlayer(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Join(string playerId, string playerNick)
        {
            if (p1 != null && p1.Id == playerId || p2 != null && p2.Id == playerId)
            {
                return new ActionResult(false, "Player already registered"); 
            }

            if (p1 != null && p2 != null)
            {
                return new ActionResult(false, "Game is full");
            }

            if (_started) return new ActionResult(false, "Game has already started");
            
            var newPlayer = new BattleshipsPlayerModel { 
                Id = playerId, 
                PlayerNick = playerNick, 
                IsHost = _config.creatorId == playerId
            };

            if (p1 == null) p1 = newPlayer;
            else p2 = newPlayer;

            return new ActionResult(true);
        }

        public ActionResult RegisterShips(List<ShipModel> shipModels, string playerId)
        {
            if (!(p1 != null && p1.Id == playerId || p2 != null && p2.Id == playerId)) return new ActionResult(false, "not registered");
            _game.Register(shipModels, playerId);

            return new ActionResult(true);
        }

        public ActionResult Move(string playerId, BattleshipsPosition move)
        {
            if (!_started)
            {
                return new ActionResult(false, "Easy, tiger! The game hasn't started yet!");
            }
    
            var player = _nextTurnPlayerIndex == 1 ? p1 : p2;

            if (player.Id != playerId)
            {
                return new ActionResult(false, "It's not your turn");
            }

            if (!IsMoveInBounds(move))
            {
                return new ActionResult(false, "Move out of bounds");
            }

            var moveResult = _game.Move(move);

            if (moveResult.WasSuccessful) 
            {
                _nextTurnPlayerIndex *= -1;
            }

            var message = GenerateMoveString(moveResult);

            return new BattleshipsMoveResult(moveResult.WasSuccessful, message, moveResult.DidEndGame);
        }

        private bool IsMoveInBounds(BattleshipsPosition move)
        {
            return !(move.row >= _config.rows || move.row < 0 || move.col >= _config.cols || move.col < 0);
        }

        private string GenerateMoveString(BattleshipsMoveResult result)
        {
            if (!result.WasSuccessful) return "Already moved there!";
            
            // if (result.HitShip == null)
            // {
            //     return "Miss";
            // }
            // else
            // {
            //     if (result.HitShip.IsSunk())
            //     {
            //         if (result.DidEndGame) return "Hit, sink and game!";
                  
            //         return "Hit and sink";

                    
            //     }
            //     else
            //     {
            //        return "Hit";
            //     }
                
            // }

            return "as valid as a cake on cakeday";
        }

        public ActionResult Resign(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Restart(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Start(string playerId)
        {
            _started = true;
            return new ActionResult(true);
        }

        private Square[,] GetGrid(Player player)
        {
            if (player == null) return new Square[_config.rows, _config.cols];

            else return player.Board._grid;
        }

        private List<Ship> GetShips(Player player)
        {
            if (player == null || player.Ships == null) return new List<Ship>();

            else return player.Ships;
        }

        public BattleshipsGameState GetGameState(string playerId)
        {
            Player player;
            Player opponent;
            

            var plonker = _nextTurnPlayerIndex == 1 ? p1 : p2;

            if (_game.p1 != null && _game.p1.PlayerId == playerId)
            {
                player = _game.p1;
                opponent = _game.p2;
            }
            else
            {
                player = _game.p2;
                opponent = _game.p1;
            }

            return new BattleshipsGameState
            {
                PlayerShips = GetShips(player),
                PlayerBoard = GetGrid(player),
                OpponentSunkShips = GetShips(opponent).FindAll(s => s.IsSunk()),
                OpponentBoard = GetGrid(opponent),
                Configuration = _config, 
                CurrentTurnPlayer = plonker, 
                Status = new GameProgress("abc", "123")    
            };
        }
    }
}