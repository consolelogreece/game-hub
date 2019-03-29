using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFour : IConnectFour
    {
        private string[][] _board;

        private int _winThreshold = 4;

        private int _rowCount = 6;

        private int _columnCount = 7;

        private int _nextPlayerIndex;

        private string[] _players;

        private bool _gameOver = false;

        public ConnectFour()
        {
            _players = new string[] { "Red", "Yellow" };

            // Initialize board
            _board = new string[_rowCount][];

            for (int i = 0; i < _rowCount; i++)
            {
                string[] row = new string[_columnCount];
                _board[i] = row;
            }
        }


        public MoveResult MakeMove(int col, string player)
        {
            // create own exception?
            if (_gameOver) throw new Exception("Game is over");

            if (_players[_nextPlayerIndex] != player)
            {
                return new MoveResult()
                {
                    WasValidMove = false,
                    Message = "It is not your turn."
                };
            }

            var row = FindRow(col);

            var MoveResult = new MoveResult()
            {
                player = player,
                row = row,
                col = col   
            };

            if (row != -1)
            {
                _board[row][col] = player;

                // update next player, loop back to start if last player in list.
                _nextPlayerIndex = (_nextPlayerIndex + 1) % _players.Length;

                MoveResult.WasValidMove = true;
            }

            var didWin = HasWon(row, col);

            MoveResult.DidMoveWin = didWin;

            _gameOver = didWin;

            return MoveResult;
        }

        private bool HasWon(int row, int col)
        {
            return CheckVertical(row, col) || CheckHorizontal(row, col) || CheckDiagonalTLBR(row, col) || CheckDiagonalTRBL(row, col);
        }

        private int FindRow(int col)
        {
            int i = -1;
            foreach (var row in _board)
            {
                i++;
                if (row[col] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool CheckVertical(int row, int col)
        {
            int count = 1;

            var player = _board[row][col];

            row--;

            for (; row >= 0; row--)
            {
                if (_board[row][col] == player) count++;
                else break;
            }

            return count >= _winThreshold;
        }

        private bool CheckHorizontal(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var player = _board[row][col];

            while (++tempCol < _columnCount && _board[row][tempCol] == player) count++;

            while (--col >= 0 && _board[row][col] == player) count++;

            return count >= _winThreshold;
        }

        private bool CheckDiagonalTLBR(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var tempRow = row;

            var player = _board[row][col];

            while (++tempCol < _columnCount && ++tempRow < _rowCount && _board[tempRow][tempCol] == player) count++;

            while (--col >= 0 && --row >= 0 && _board[row][col] == player) count++;

            return count >= _winThreshold;
        }

        private bool CheckDiagonalTRBL(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var tempRow = row;

            var player = _board[row][col];

            while (++tempCol < _columnCount && --tempRow >= 0 && _board[tempRow][tempCol] == player) count++;

            while (--col >= 0 && ++row < _rowCount && _board[row][col] == player) count++;

            return count >= _winThreshold;
        }
    }
}
