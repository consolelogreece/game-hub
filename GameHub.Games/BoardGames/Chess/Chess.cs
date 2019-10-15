using System.Collections.Generic;
using System.Linq;
using ChessDotNet;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Chess
{
    public class Chess : IBoardGame
    {
        private ChessGame _game = new ChessGame();

        private ChessPlayer White;

        private ChessPlayer Black;

        private bool _started = false;

        private ChessConfig _config;

        public Chess(ChessConfig config)
        {
           _config = config;
        }

        public bool MakeMove(string playerId, Move move)
        {
            var result = _game.MakeMove(move, false);

            return result != MoveType.Invalid;
        }

        private GameProgress GetGameStatus()
        {
            var IsStalemated = (White != null && _game.IsStalemated(White.player) || Black != null && _game.IsStalemated(Black.player));

            var winner = GetWinner();

            var playerResigned = _game.Resigned;

            var isOver = false;

            var endReason = "";

            
            if (IsStalemated)
            {
                isOver = true;
                endReason = "Stalemate";
            }
            else if(playerResigned != Player.None)
            {
                isOver = true;
                var resignedPlayer = this.GetPlayer(playerResigned);

                endReason = resignedPlayer.PlayerNick + " resigned";
            }
            else if (winner != null)
            {
                isOver = true;
                endReason = winner.PlayerNick + " has won!";
            }

            var status = (isOver ? Common.GameStatus.finished : _started ? Common.GameStatus.started : Common.GameStatus.lobby).ToString();
            
            var result = new GameProgress(status, endReason);

            return result;
        }

        private ChessPlayer GetWinner()
        {
            ChessPlayer winner = null;

            if (White != null && _game.IsWinner(White.player)) winner = White;
            else if (Black != null && _game.IsWinner(Black.player)) winner = Black;

            return winner;
        }

        public bool StartGame(string playerId)
        {
            // only game creator can start the game.
            if (playerId != _config.creatorId) return false;

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
                IsHost = playerId == _config.creatorId
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

        public ChessPlayer GetPlayer(Player player)
        {
            if (White == null) return null;

            if (player == Player.White) return White;

            if (Black == null) return null;
        
            if (player == Player.Black) return Black;
            
            return null;
        }

        public List<Move> GetMoves(ChessPlayer ChessPlayer)
        {
            var moves = _game.GetValidMoves(ChessPlayer.player);

            return moves.ToList();
        }

        public bool Reset(string playerId)
        {
            if( _config.creatorId != playerId) return false;

            // cant rematch if the game hasn't even started.
            if (!_started) return false;

            _game = new ChessGame();

            return true;
        }

        public bool Resign(string playerId)
        {
            var player = this.GetPlayer(playerId);

            _game.Resign(player.player);

            return true;
        }

        public GameState GetGameState()
        {
            var currentTurnPlayer =  this.GetPlayer(_game.WhoseTurn);
            
            return new GameStateChess
            {
                BoardStateFen = _game.GetFen(),
                CurrentTurnPlayer = currentTurnPlayer,
                Status = GetGameStatus()
            };
        }
    }
}