using System.Collections.Generic;
using System.Linq;
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

        private bool _gameOver;

        #endregion

        public Battleships(BattleshipsConfiguration config)
        {
            _config = config;

            _game = new BattleshipsGame(config.Rows, _config.Cols);
        }

        private bool AreShipsOverlapping(List<ShipModel> ships)
        {
            var occupiedSquares = new HashSet<BattleshipsPosition>();

            foreach(var ship in ships)
            {
                if (ship.orientation == Orientation.Horizontal)
                {
                    for(int i = 0; i < ship.length; i++)
                    {
                        var row = ship.row;
                        var col = ship.col + i;
                        
                        var position = new BattleshipsPosition(row, col);

                        var exists = !occupiedSquares.Add(position);
                        if (exists) return true;
                    }
                }
                else
                {
                    for(int i = 0; i < ship.length; i++)
                    {
                        var row = ship.row + i;
                        var col = ship.col;

                        var position = new BattleshipsPosition(row, col);

                        var exists = !occupiedSquares.Add(position);
                        if (exists) return true;
                    }
                }
            }

            return false;
        }

        public bool AreShipsInBoardBounds(List<ShipModel> ships)
        {
            foreach(var ship in ships)
            {
                if (ship.orientation == Orientation.Horizontal)
                {
                    for(int i = 0; i < ship.length; i++)
                    {
                        var row = ship.row;
                        var col = ship.col + i;
                        if (row < 0 || row >= _config.Rows || col < 0 || col >= _config.Cols)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < ship.length; i++)
                    {
                        var row = ship.row + i;
                        var col = ship.col;
                        if (row < 0 || row >= _config.Rows || col < 0 || col >= _config.Cols)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool AreShipsCompliant(List<ShipModel> ships)
        {
            foreach(var ship in ships)
            {
                if (!_config.InitialShipLayout.Any(s => AreShipsEqual(s, ship)))
                {
                    return false;
                }
            }

            return true;
        }

        // excluding orientation and position as irrelevant
        private bool AreShipsEqual(ShipModel ship1, ShipModel ship2)
        {
            return ship1.Id == ship2.Id && ship1.length == ship2.length;
        }

        public ActionResult ValidateShips(List<ShipModel> ships)
        {
            if (AreShipsOverlapping(ships)) return new ActionResult(false, "Ships overlapping, stupid");
            if (!AreShipsInBoardBounds(ships)) return new ActionResult(false, "One or more ships is outside the bounds of the board, stupid");
            if (!AreShipsCompliant(ships)) return new ActionResult(false, "Oops, your ships don't match this games ship configuration");

            return new ActionResult(true);
        }

        public BattleshipsGameState GetGameState()
        {
            throw new System.NotImplementedException();
        }

        public BattleshipsPlayerModel GetPlayer(string playerId)
        {
            if (p1 != null && p1.Id == playerId) return p1;
            if (p2 != null && p2.Id == playerId) return p2;
            return null;
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
                IsHost = _config.CreatorId == playerId
            };

            if (p1 == null) p1 = newPlayer;
            else p2 = newPlayer;

            return new ActionResult(true);
        }

        public ActionResult RegisterShips(List<ShipModel> shipModels, string playerId)
        {
            if (!(p1 != null && p1.Id == playerId || p2 != null && p2.Id == playerId)) return new ActionResult(false, "not registered");
            if (_game.p1 != null && _game.p1.PlayerId == playerId || _game.p2 != null && _game.p2.PlayerId == playerId) return new ActionResult(false, "Already submitted shiperinos");
            
            var validation = ValidateShips(shipModels);
            if (!validation.WasSuccessful) return validation;

            var playerNumber = p1.Id == playerId ? PlayerNumber.One : PlayerNumber.Two;

            _game.Register(shipModels, playerId, playerNumber);

            GetPlayer(playerId).Ready = true;

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

            if (moveResult.DidEndGame)
            {
                _gameOver = true;
            }

            return new BattleshipsMoveResult(moveResult.WasSuccessful, message, moveResult.DidEndGame);
        }

        private bool IsMoveInBounds(BattleshipsPosition move)
        {
            return !(move.row >= _config.Rows || move.row < 0 || move.col >= _config.Cols || move.col < 0);
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
            if (playerId != _config.CreatorId) return new ActionResult(false,  "You are not the host");

            if (_started) return new ActionResult(false, "The game has already started");

            if (p1 == null || p2 == null) return new ActionResult(false, "Not enough players");

            if (_game.p1 == null || _game.p2 == null)  return new ActionResult(false, "Not all players are ready");

            _started = true;
            return new ActionResult(true);
        }

        private Square[,] GetGrid(Player player)
        {
            if (player == null) return new Square[_config.Rows, _config.Cols];

            else return player.Board._grid;
        }

        private List<Ship> GetShips(Player player)
        {
            if (player == null || player.Ships == null) return new List<Ship>();

            else return player.Ships;
        }

        private GameProgress GetGameStatus()
        {
            var endReason = "";

            var status = (_gameOver ? GameStatus.finished : _started ? GameStatus.started : GameStatus.lobby).ToString();

            if (_gameOver)
            {
                var winner = _game.Winner;

                if (winner != null) endReason = GetPlayer(winner.PlayerId).PlayerNick + " has won!";
                else endReason = "It's a draw!";
            }

            return new GameProgress(status, endReason);
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
                Status = this.GetGameStatus()  
            };
        }
    }
}