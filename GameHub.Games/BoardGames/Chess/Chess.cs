using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChessDotNet;
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

        void MakeMove(string playerId, Move move)
        {
            var player = GetPlayer(playerId);

            var result = _game.MakeMove(move, false);
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
                else if (Black == null)
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
            return new GameState
            {
                BoardStateFen = _game.GetFen()
            };
        }
    }
}