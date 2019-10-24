using System.Collections.Generic;
using System.Linq;
using ChessDotNet;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Chess
{
    public class Chess : IBoardGame<GameStateChess, ChessPlayer>
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

        public ActionResult StartGame(string playerId)
        {
            // only game creator can start the game.
            if (playerId != _config.creatorId) return new ActionResult(false,  "You are not the host");

            if (_started) return new ActionResult(false, "The game has already started");

            if (Black == null || White == null) return new ActionResult(false, "Not enough players to start");
            else _started = true;

            return new ActionResult(true);
        }

        public ActionResult RegisterPlayer(string playerId, string playerNick)
        {
            var cp = new ChessPlayer
            {
                Id = playerId,
                PlayerNick = playerNick,
                IsHost = playerId == _config.creatorId
            };

            var isWhiteTaken = White != null;
            var isBlackTaken = Black != null;

            if ((isBlackTaken && Black.PlayerNick == playerNick) || (isWhiteTaken && White.PlayerNick == playerNick))
            {
                return new ActionResult(false, "Name already in use");
            }

            lock(_game)
            {
                if (!isWhiteTaken)
                {
                    cp.player = Player.White;

                    White = cp;
                }
                else if (!isBlackTaken && White.Id != playerId)
                {
                    cp.player = Player.Black;

                    Black = cp;
                }
                else
                {
                    return new ActionResult(false, "Game is full");
                }

                return new ActionResult(true);
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

        public ActionResult Reset(string playerId)
        {
            if( _config.creatorId != playerId) return new ActionResult(false, "You are not the host");

            // cant rematch if the game hasn't even started.
            if (!_started) return new ActionResult(false, "Game hasn't started");

            _game = new ChessGame();

            return new ActionResult(true);
        }

        public ActionResult Resign(string playerId)
        {
            var player = this.GetPlayer(playerId);

            _game.Resign(player.player);

            return new ActionResult(true);
        }

        public GameStateChess GetGameState()
        {
            var currentTurnPlayer =  this.GetPlayer(_game.WhoseTurn);

            var players = new List<ChessPlayer>();

            if (White != null) players.Add(White);
            if (Black != null) players.Add(Black);
            
            return new GameStateChess
            {
                BoardStateFen = _game.GetFen(),
                CurrentTurnPlayer = currentTurnPlayer,
                Status = GetGameStatus(),
                Configuration = _config,
                Players = players
            };
        }
    }
}