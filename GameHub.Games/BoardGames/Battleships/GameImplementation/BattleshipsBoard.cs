using System;


namespace GameHub.Games.BoardGames.Battleships
{
    public class Board
    {
        int _rows, _cols;
        Square[,] grid;
        public Board(int rows, int cols)
        {
            _rows = rows; _cols = cols;

            grid = new Square[_rows, _cols];

            populateSquares();
        }

        private void populateSquares()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    grid[i,j] = new Square();
                }
            }
        }

        public void MapShip(Ship ship)
        {
            if (ship.orientation == Orientation.Horizontal)
            {
                for(int i = 0; i < ship.length; i++)
                {
                    var row = ship.row;
                    var col = ship.col + i;
                    if (IsOutsideBoundaries(row,  col))
                    {
                        throw new IndexOutOfRangeException("invalid ship position");
                    }
                    
                    grid[row, col].occupyingShip = ship;
                }
            }
            else
            {
                for(int i = 0; i < ship.length; i++)
                {
                    var row = ship.row + i;
                    var col = ship.col;
                    if (IsOutsideBoundaries(row,  col))
                    {
                        throw new IndexOutOfRangeException("invalid ship position");
                    }

                    grid[row, col].occupyingShip = ship;
                }
            }
        }

        private bool IsOutsideBoundaries(int row, int col)
        {
            return row < 0 || row >= _rows || col < 0 || row >= _cols;
        }

        public Square this[int row, int col] => grid[row,col];
    }
}