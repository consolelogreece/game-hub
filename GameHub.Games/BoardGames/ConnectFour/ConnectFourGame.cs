using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourGame
    {
        public string[][] _board { get; private set; }

        private string _winnerID;

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

        public ActionResult MakeMove(int col, string playerId)
        {
            var wasValidMove = false;

            var message = "";

            if (col >= _columnCount || _columnCount < 0)
            {
                wasValidMove = false;

                message = "Invalid column";

                return new ActionResult(wasValidMove, message);
            }

            var row = FindRow(col);

            if (row == -1) {
                wasValidMove = false;

                message = "That column is full.";

                return new ActionResult(wasValidMove, message);
            }

            wasValidMove = true;

            _spacesLeft--;

            _board[row][col] = playerId;

            if (HasWon(row, col))
            {
                _winnerID = playerId;
            }

            return new ActionResult(wasValidMove, message);
        }

        private bool HasWon(int row, int col)
        {
            return CheckVertical(row, col) || CheckHorizontal(row, col) || CheckDiagonalTLBR(row, col) || CheckDiagonalTRBL(row, col);
        }

        public bool IsDraw()
        {
            return _spacesLeft == 0;
        }

        public string GetWinnerID()
        {
            return _winnerID;
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
