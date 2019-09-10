using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFour : IConnectFour
    {
        private ConnectFourGame _game;

        private List<ConnectFourPlayer> _players;

        private string _creatorId;

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

        private int _maxPlayers = 8;

        public ConnectFour(ConnectFourConfiguration config)
        {
            _maxPlayers = config.nPlayersMax;

            _creatorId = config.creatorId;

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
                var nextTurnPlayer = _players[_nextPlayerIndex];

                if (nextTurnPlayer.Id != playerId)
                {
                    error.Message = "It is not your turn.";
                    return error;
                }

                var moveResult = _game.MakeMove(col, playerId);

                moveResult.NextTurnPlayer = nextTurnPlayer.PlayerNick;

                moveResult.PlayerColor = nextTurnPlayer.PlayerColor;

                moveResult.BoardState = GetBoardStateColors();
              
                if (moveResult.DidMoveWin)
                {
                    // todo sanitize nickname
                    moveResult.Message = $"{nextTurnPlayer.PlayerNick} won!";
                }

                _nextPlayerIndex = (_nextPlayerIndex + 1) % _players.Count;

                return moveResult;
            }      
        }

        public bool RegisterPlayer(string playerId, string playerNick)
        {
            var newPlayer = new ConnectFourPlayer { Id = playerId, PlayerNick = playerNick, PlayerColor = _colors[_players.Count] };

            lock (_players)
            lock(_game)
            {
                if (_players.Count > _maxPlayers || _players.Any(p => p.Id == newPlayer.Id))
                {
                    return false;
                }

                _players.Add(newPlayer);

                return true;
            }
        }

        public bool Start(string playerId)
        {
            if (_players.Count == 0) return false;

            var gameCreator = _players[0];

            if (gameCreator != null && gameCreator.Id == playerId)
            {
                _gameStarted = true;
            }

            return _gameStarted;
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

        public GameState GetGameState(string playerId)
        {
            lock (_players)
            {
                var player = _players.FirstOrDefault(p => p.Id == playerId);

                var gameState = new GameState();

                var status = _gameOver ? GameStatus.finished : _gameStarted ? GameStatus.started : GameStatus.lobby;

                gameState.Status = status.ToString();

                gameState.BoardState = GetBoardStateColors();

                gameState.IsGameCreator = _creatorId == playerId;

                gameState.Players = _players;

                if (player != null)
                {
                    gameState.IsPlayerRegistered = true;
                    gameState.RegisteredNick = player.PlayerNick;      
                }

                return gameState;
            }
        }
    }
}
