using System.Collections.Generic;
using System.Linq;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFour : IBoardGame<GameStateConnectFour, ConnectFourPlayer>
    {
        #region private props
        private ConnectFourGame _game;

        private List<ConnectFourPlayer> _players;

        private List<string> _colors = new List<string>()
        {
            "red",
            "yellow",
            "cyan",
            "limegreen",
            "magenta",
            "orange",
            "purple",
            "lightblue"
        };

        private int _currentPlayerIndex;

        private bool _gameOver = false;

        private ConnectFourPlayer _winner;

        private bool _started = false;

        private ConnectFourConfiguration _config;
        #endregion
        public ConnectFour(ConnectFourConfiguration config)
        {
            _config = config;

            _game = new ConnectFourGame(config);

            _players = new List<ConnectFourPlayer>();
        }

        public ActionResult MakeMove(int col, string playerId)
        {
            if (!_started)
            {
                return new ActionResult(false, "Easy, tiger! The game hasn't started yet!");
            }
            
            if (_gameOver)
            {
                return new  ActionResult(false, "The game has finished!");
            }

            var player = _players[_currentPlayerIndex];

            if (player.Id != playerId)
            {
                return new ActionResult(false, "It's not your turn");
            }

            var moveResult = _game.MakeMove(col, player.PlayerColor);

            if (!moveResult.WasSuccessful) return moveResult;
            
            UpdateMoveMade();

            return new ActionResult(true);
        }

        private void UpdateMoveMade()
        {
            UpdateWinConventional();

            UpdatePlayerTurn();
        }

        private void UpdateWinConventional()
        {
            var gameWinnerColor = _game.GetWinner();

            if (gameWinnerColor != null) 
            {
                _winner = _players.FirstOrDefault(p => p.PlayerColor == gameWinnerColor);

                _winner.Wins++;

                _gameOver = true;
            }
        }

        private void UpdatePlayerTurn()
        {
            _currentPlayerIndex = FindIndexOfNextPlayerToMove();
        }

        private void UpdateResigned()
        {
            UpdateWinByResignations();

            // If the move indicator points to a player who's resigned, udate.
            if (_players[_currentPlayerIndex].Resigned) UpdatePlayerTurn();
        }

        private void UpdateWinByResignations()
        {
            if (_players.Count(p => !p.Resigned) < 2)
            {
                _gameOver = true;
                _winner = _players.FirstOrDefault(p => !p.Resigned);
                _winner.Wins++;
            }
        }

        private int FindIndexOfNextPlayerToMove()
        {
            var nPlayers = _players.Count;

            var currentPlayerIndexCopy = _currentPlayerIndex;

            var loopCounter = 0;

            var index = -1;

            while(loopCounter < nPlayers)
            {
                loopCounter++;

                currentPlayerIndexCopy = ((currentPlayerIndexCopy + 1) % nPlayers);

                if (!_players[currentPlayerIndexCopy].Resigned) 
                {
                    index = currentPlayerIndexCopy;

                    break;
                }
            }

            return currentPlayerIndexCopy;
        }

        public ConnectFourPlayer GetPlayer(string playerId)
        {
            return _players.FirstOrDefault(p => p.Id == playerId);    
        }

        public ActionResult RegisterPlayer(string playerId, string playerNick)
        {
            if (_players.Any(p => p.Id == playerId))
            {
                return new ActionResult(false, "Player already registered"); 
            }

            if (_players.Any(p => p.PlayerNick == playerNick))
            {
                return new ActionResult(false, "Name already in use");
            }

            if (_players.Count >= _config.nPlayersMax)
            {
                return new ActionResult(false, "Game is full");
            }

            if (_started) return new ActionResult(false, "Game has already started");
            
            var newPlayer = new ConnectFourPlayer { 
                Id = playerId, 
                PlayerNick = playerNick, 
                PlayerColor = _colors[_players.Count], 
                IsHost = _config.creatorId == playerId
            };

            _players.Add(newPlayer);

            return new ActionResult(true);
        }

        public ActionResult StartGame(string playerId)
        {
            if (_players.Count < 2) return new ActionResult(false, "Not enough players");

            if (playerId != _config.creatorId) return new ActionResult(false,  "You are not the host");

            if (_started) return new ActionResult(false, "The game has already started");

            _started = true;

            return new ActionResult(true);
        }

        public ActionResult Reset(string playerId)
        {
            if(_config.creatorId != playerId) return new ActionResult(false,  "You are not the host");

            _game.ClearBoard();

            _gameOver = false;

            _started = true;

            _currentPlayerIndex = 0;

            // un-resign resigned players
            _players.ForEach(p => p.Resigned = false);

            return new ActionResult(true);
        }

        private GameProgress GetGameStatus()
        {
            var endReason = "";

            var status = (_gameOver ? GameStatus.finished : _started ? GameStatus.started : GameStatus.lobby).ToString();

            if (_gameOver)
            {
                if (_winner != null) endReason = _winner.PlayerNick + " has won!";
                else endReason = "It's a draw!";
            }

            return new GameProgress(status, endReason);
        }

        public GameStateConnectFour GetGameState()
        {
            var gameState = new GameStateConnectFour();

            gameState.Status = this.GetGameStatus();

            gameState.BoardState = _game.Board;

            gameState.Players = _players;

            gameState.CurrentTurnPlayer = _players.Count != 0 ? _players[_currentPlayerIndex] : null;

            gameState.Configuration = this._config;

            return gameState;
        }

        public ActionResult Resign(string playerId)
        {
            var player = this.GetPlayer(playerId);

            // cant resign if player does not exist.
            if (player == null || !_started) return new ActionResult(false, "Not a player");

            if (player.Resigned) return new ActionResult(false, "Player already resigned");

            if (_gameOver) return new ActionResult(false, "Game is over");

            if (!_started) return new ActionResult(false, "Game hasn't started");

            player.Resigned = true;

            UpdateResigned();

            return new ActionResult(true);
        }
    }
}