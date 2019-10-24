using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourGame
    {
        public string[][] Board { get; private set; }

        private string _winner;

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
            Board = new string[_rowCount][];

            for (int i = 0; i < _rowCount; i++)
            {   
                var row = new string[_columnCount];
                for (int j = 0; j < row.Length; j++ ) {
                    row[j] = "white"; // white == default
                }
                Board[i] = row;
            }
        }

        public ActionResult MakeMove(int col, string color)
        {
            var wasSuccessfulMove = false;

            var message = "";

            if (col >= _columnCount || _columnCount < 0)
            {
                wasSuccessfulMove = false;

                message = "Invalid column";

                return new ActionResult(wasSuccessfulMove, message);
            }

            var row = FindRow(col);

            if (row == -1) {
                wasSuccessfulMove = false;

                message = "That column is full.";

                return new ActionResult(wasSuccessfulMove, message);
            }

            wasSuccessfulMove = true;

            _spacesLeft--;

            Board[row][col] = color;

            if (HasWon(row, col))
            {
                _winner =  color;
            }

            return new ActionResult(wasSuccessfulMove, message);
        }

        private bool HasWon(int row, int col)
        {
            return CheckVertical(row, col) || CheckHorizontal(row, col) || CheckDiagonalTLBR(row, col) || CheckDiagonalTRBL(row, col);
        }

        public bool IsDraw()
        {
            return _spacesLeft == 0;
        }

        public string GetWinner()
        {
            return _winner;
        }

        private int FindRow(int col)
        {
            int i = -1;
            foreach (var row in Board)
            {
                i++;
                if (row[col] == "white")
                {
                    return i;
                }
            }

            return -1;
        }

        private bool CheckVertical(int row, int col)
        {
            int count = 1;

            var player = Board[row][col];

            row--;

            for (; row >= 0; row--)
            {
                if (Board[row][col] == player) count++;
                else break;
            }

            return count >= _winThreshold;
        }

        private bool CheckHorizontal(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var player = Board[row][col];

            while (++tempCol < _columnCount && Board[row][tempCol] == player) count++;

            while (--col >= 0 && Board[row][col] == player) count++;

            return count >= _winThreshold;
        }

        private bool CheckDiagonalTLBR(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var tempRow = row;

            var player = Board[row][col];

            while (++tempCol < _columnCount && ++tempRow < _rowCount && Board[tempRow][tempCol] == player) count++;

            while (--col >= 0 && --row >= 0 && Board[row][col] == player) count++;

            return count >= _winThreshold;
        }

        private bool CheckDiagonalTRBL(int row, int col)
        {
            int count = 1;

            var tempCol = col;

            var tempRow = row;

            var player = Board[row][col];

            while (++tempCol < _columnCount && --tempRow >= 0 && Board[tempRow][tempCol] == player) count++;

            while (--col >= 0 && ++row < _rowCount && Board[row][col] == player) count++;

            return count >= _winThreshold;
        }

        public string[][] GetBoardState()
        {
            return Board;
        }

        public void ClearBoard()
        {
            InitializeBoard();
        }
    }
}
