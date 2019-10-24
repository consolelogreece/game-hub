﻿using System.Collections.Generic;
using System.Linq;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFour : IBoardGame<GameStateConnectFour, ConnectFourPlayer>
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

        private ConnectFourPlayer _winner;

        private bool _gameStarted = false;

        private ConnectFourConfiguration _config;

        public ConnectFour(ConnectFourConfiguration config)
        {
            _config = config;

            _game = new ConnectFourGame(config);

            _players = new List<ConnectFourPlayer>();
        }

        public ActionResult MakeMove(int col, string playerId)
        {
            if (!_gameStarted)
            {
                return new ActionResult(false, "Easy, tiger! The game hasn't started yet!");
            }
            
            if (_gameOver)
            {
                return new  ActionResult(false, "The game has finished!");
            }

            lock (_game)
            lock (_players)
            {
                var player = _players[_nextPlayerIndex];

                if (player.Id != playerId)
                {
                    return new ActionResult(false, "It's not your turn");
                }

                var moveResult = _game.MakeMove(col, playerId);

                if (!moveResult.WasSuccessful) return moveResult;

                _nextPlayerIndex = (_nextPlayerIndex + 1) % _players.Count;

                UpdateStatus();

                return new ActionResult(true);
            }
        }

        private void UpdateStatus()
        {
            var gameWinnerID = _game.GetWinnerID();

            if (gameWinnerID != null) 
            {
                _winner = GetPlayer(gameWinnerID);

                _winner.Wins++;

                _gameOver = true;
            }
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

            if (_gameStarted) return new ActionResult(false, "Game has already started");
            
            var newPlayer = new ConnectFourPlayer { 
                Id = playerId, 
                PlayerNick = playerNick, 
                PlayerColor = _colors[_players.Count], 
                IsHost = _config.creatorId == playerId
            };

            lock (_players)
            {
                _players.Add(newPlayer);
            }

            return new ActionResult(true);
        }

        public ActionResult StartGame(string playerId)
        {
            if (_players.Count < 2) return new ActionResult(false, "Not enough players");

            if (playerId != _config.creatorId) return new ActionResult(false,  "You are not the host");

            if (_gameStarted) return new ActionResult(false, "The game has already started");

            _gameStarted = true;

            return new ActionResult(true);
        }

        // returns true if reset was successful.
        public ActionResult Reset(string playerId)
        {
            if( _config.creatorId != playerId) return new ActionResult(false,  "You are not the host");;

            _game.ClearBoard();

            _gameOver = false;

            _gameStarted = true;

            _nextPlayerIndex = 0;

            return new ActionResult(true);
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

        private GameProgress GetGameStatus()
        {
            var endReason = "";

            var status = (_gameOver ? GameStatus.finished : _gameStarted ? GameStatus.started : GameStatus.lobby).ToString();

            if (_gameOver)
            {
                if (_winner != null) endReason = _winner.PlayerNick + " has won!";
                else endReason = "It's a draw!";
            }

            return new GameProgress(status, endReason);
        }

        public GameStateConnectFour GetGameState()
        {
            lock (_players)
            {
                var gameState = new GameStateConnectFour();

                gameState.Status = this.GetGameStatus();

                gameState.BoardState = GetBoardStateColors();

                gameState.Players = _players;

                gameState.CurrentTurnPlayer = _players.Count != 0 ? _players[_nextPlayerIndex] : null;

                gameState.Configuration = this._config;

                return gameState;
            }
        }

        public ActionResult Resign(string playerId)
        {
            var player = this.GetPlayer(playerId);

            // cant resign if player does not exist.
            if (player == null || !_gameStarted) return new ActionResult(false, "Not a player");

            lock(_players)
            {
                _players.Remove(player);
            }

            if (_players.Count < 2)
            {
                _gameOver = true;
                _winner = _players.FirstOrDefault();
                _winner.Wins++;
            }

            return new ActionResult(true);
        }
    }
}
