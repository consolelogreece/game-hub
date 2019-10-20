using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourGame
    {
        public string[][] _board { get; private set; }

        private int _winThreshold = 4;

        private int _rowCount = 6;

        private int _columnCount = 7;

        private int _spacesLeft;

        public ConnectFourGame(ConnectFourConfiguration config)
        {
            _rowCount = config.nRows;

            _columnCount = config.nCols;

            _winThreshold = config.winThreshold;

            _spacesLeft = config.nRows * config.nCols;

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _board = new string[_rowCount][];

            for (int i = 0; i < _rowCount; i++)
            {   
                var row = new string[_columnCount];
                _board[i] = row;
            }
        }

        public MoveResult MakeMove(int col, string playerId)
        {
            var moveResult = new MoveResult();

            if (col >= _columnCount || _columnCount < 0)
            {
                moveResult.WasValidMove = false;

                moveResult.Message = "Invalid column";

                return moveResult;
            }

            var row = FindRow(col);

            if (row == -1) {
                moveResult.WasValidMove = false;

                moveResult.Message = "That column is full.";

                return moveResult;
            }

            moveResult.WasValidMove = true;

            _spacesLeft--;

            _board[row][col] = playerId;

            if (HasWon(row, col))
            {
                moveResult.DidMoveEndGame = true;
                moveResult.EndReason = "Win";
            }
            
            if (IsDraw())
            {
                moveResult.DidMoveEndGame = true;
                moveResult.EndReason = "Draw";
            }

            return moveResult;
        }

        private bool HasWon(int row, int col)
        {
            return CheckVertical(row, col) || CheckHorizontal(row, col) || CheckDiagonalTLBR(row, col) || CheckDiagonalTRBL(row, col);
        }

        private bool IsDraw()
        {
            return _spacesLeft == 0;
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

        public string[][] GetBoardState()
        {
            return _board;
        }

        public void ClearBoard()
        {
            InitializeBoard();
        }
    }
}
