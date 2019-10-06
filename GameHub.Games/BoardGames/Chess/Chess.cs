using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChessDotNet;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

namespace GameHub.Games.BoardGames.Chess
{
    public class Chess
    {
        private ChessGame _game = new ChessGame();

        private ChessPlayer White;

        private ChessPlayer Black;

        private bool _started = false;

        private string _creatorId;

        private bool _gameOver = false;

        public Chess(ChessConfig config)
        {
            _creatorId = config.creatorId;
        }

        public MoveResult MakeMove(string playerId, Move move)
        {
            var player = GetPlayer(playerId);

            var result = _game.MakeMove(move, false);

            var wasValid = result != MoveType.Invalid;

            var moveResult = new MoveResult
            {
                //GameConclusionResult = _game.IsWinner(player.player),
                Fen = _game.GetFen(),
                CurrentTurnPlayer = result == MoveType.Invalid ? player : player.player == Player.White ? Black : White,
                Message = wasValid ? "" : "INVALID MOVE",
                Player = player,
                WasValidMove = wasValid
            };

            return moveResult;
        }

        private GameConclusionResult GetConclusionResult(Player player)
        {
            return null;//bool isGameOver = _game.IsWinner(player) || _game.IsStalemated(player) || _game.;
        }

        public bool StartGame()
        {
            if (_started) return true;

            if (Black == null || White == null) _started = false;
            else _started = true;

            return _started;
        }

        public bool RegisterPlayer(string playerId, string playerNick)
        {
            var cp = new ChessPlayer
            {
                Id = playerId,
                PlayerNick = playerNick,
                IsHost = playerId == _creatorId
            };

            lock(_game)
            {
                if (White == null)
                {
                    cp.player = Player.White;

                    White = cp;
                }
                else if (Black == null && White.Id != playerId)
                {
                    cp.player = Player.Black;

                    Black = cp;
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        public ChessPlayer GetPlayer(string Id)
        {
            if (White == null) return null;

            if (White.Id == Id) return White;

            if (Black == null) return null;
        
            else if (Black.Id == Id) return Black;
            
            return null;
        }

        public List<Move> GetMoves(ChessPlayer ChessPlayer)
        {
            var moves = _game.GetValidMoves(ChessPlayer.player);

            return moves.ToList();
        }

        public GameState GetGameState()
        {
            var status = _gameOver ? GameStatus.finished : _started ? GameStatus.started : GameStatus.lobby;

            var currentTurnPlayer = _game.WhoseTurn == Player.White ? White : Black;
            
            return new GameState
            {
                BoardStateFen = _game.GetFen(),
                Status = status.ToString(),
                CurrentTurnPlayer = currentTurnPlayer
            };
        }
    }
}