using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFour : IBoardGame
    {
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

        private int _nextPlayerIndex;

        private bool _gameOver = false;

        private bool _gameStarted = false;

        private ConnectFourConfiguration _config;

        public ConnectFour(ConnectFourConfiguration config)
        {
            _config = config;

            _game = new ConnectFourGame(config);

            _players = new List<ConnectFourPlayer>();
        }

        public MoveResult MakeMove(int col, string playerId)
        {
            var error = new MoveResult();

            if (_gameStarted == false)
            {
                error.Message = "Game has not started";
                return error;
            }

            // todo check for draws
            if (_gameOver)
            {
                error.Message = "Game is over";
                return error;
            };
   
            lock (_game)
            lock (_players)
            {
                var player = _players[_nextPlayerIndex];

                if (player.Id != playerId)
                {
                    error.Message = "It is not your turn.";
                    return error;
                }

                var moveResult = _game.MakeMove(col, playerId);

                moveResult.Player = _players.FirstOrDefault(p => p.Id == playerId);

                _nextPlayerIndex = (_nextPlayerIndex + 1) % _players.Count;

                moveResult.NextTurnPlayer = _players[_nextPlayerIndex];

                moveResult.BoardState = GetBoardStateColors();
              
                if (moveResult.DidMoveWin)
                {
                    // TODO: sanitize nickname
                    moveResult.Message = $"{player.PlayerNick} won!";
                    _gameOver = true;
                    player.Wins++;
                }

                return moveResult;
            }      
        }

        public ConnectFourPlayer GetPlayer(string playerId)
        {
            return _players.FirstOrDefault(p => p.Id == playerId);    
        }

        public bool RegisterPlayer(string playerId, string playerNick)
        {
            var newPlayer = new ConnectFourPlayer { Id = playerId, 
            PlayerNick = playerNick, 
            PlayerColor = _colors[_players.Count], 
            IsHost = _config.creatorId == playerId};

            lock (_players)
            lock(_game)
            {
                if (_players.Count > _config.nPlayersMax || _players.Any(p => p.Id == newPlayer.Id))
                {
                    return false;
                }

                _players.Add(newPlayer);

                return true;
            }
        }

        public bool StartGame(string playerId)
        {
            if (_players.Count == 0) return false;

            if (_config.creatorId == playerId)
            {
                _gameStarted = true;
            }

            return _gameStarted;
        }

        // returns true if reset was successful.
        public bool Reset(string playerId)
        {
            if( _config.creatorId != playerId) return false;

            _game.ClearBoard();

            _gameOver = false;

            _gameStarted = true;

            _nextPlayerIndex = 0;

            return true;
        }

        public string[][] GetBoardStateColors()
        {
            lock (_game)
            {
                var board = _game.GetBoardState();

                var boardColorsOnly = new string[board.GetLength(0)][];

                for (int i = 0; i < board.GetLength(0); i++)
                {
                    var formattedRow = new string[board[i].Length];

                    for (int j = 0; j < board[i].Length; j++)
                    {
                        var playerId = board[i][j];
                        var color = _players.FirstOrDefault(p => p.Id == playerId)?.PlayerColor ?? "white";
                        formattedRow[j] = color;
                    }

                    boardColorsOnly[i] = formattedRow;
                }

                return boardColorsOnly;
            }
        }

        public GameStateBase GetGameState()
        {
            lock (_players)
            {
                var gameState = new GameState();

                var status = _gameOver ? GameStatus.finished : _gameStarted ? GameStatus.started : GameStatus.lobby;

                //gameState.Status = status.ToString();

                gameState.BoardState = GetBoardStateColors();

                gameState.Players = _players;

                gameState.NextTurnPlayer = _players.Count != 0 ? _players[_nextPlayerIndex] : null;

                gameState.Configuration = this._config;

                return gameState;
            }
        }

        public bool Resign(string playerId)
        {
            throw new NotImplementedException();
        }

        GameStateBase IBoardGame.GetGameState()
        {
            throw new NotImplementedException();
        }
    }
}
