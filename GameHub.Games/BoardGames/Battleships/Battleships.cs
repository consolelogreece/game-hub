using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class Battleships : IJoinable, 
        IRestartable, 
        IStartable, 
        IResignable, 
        IMoveable<BattleshipsMove>,
        IGamePlayerGetter<BattleshipsPlayerModel>, 
        IGameStateGetter<BattleshipsGameState>
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

        public ActionResult Move(string playerId, BattleshipsMove move)
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

            return new BattleshipsMoveResult(moveResult.WasSuccessful, message, moveResult.HitShip, moveResult.DidEndGame);
        }

        private bool IsMoveInBounds(BattleshipsMove move)
        {
            return !(move.row >= _config.rows || move.row < 0 || move.col >= _config.cols || move.col < 0);
        }

        private string GenerateMoveString(BattleshipsMoveResult result)
        {
            if (!result.WasSuccessful) return "Already moved there!";
            
            if (result.HitShip == null)
            {
                return "Miss";
            }
            else
            {
                if (result.HitShip.IsSunk())
                {
                    if (result.DidEndGame) return "Hit, sink and game!";
                  
                    return "Hit and sink";

                    
                }
                else
                {
                   return "Hit";
                }
                
            }
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

        BattleshipsGameState IGameStateGetter<BattleshipsGameState>.GetGameState()
        {
            throw new System.NotImplementedException();
        }
    }
}