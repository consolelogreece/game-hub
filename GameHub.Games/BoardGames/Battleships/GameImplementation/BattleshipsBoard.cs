using System;
using System.Collections.Generic;

namespace GameHub.Games.BoardGames.Battleships
{
    public class Board
    {
        int _rows, _cols;
        public Square[,] _grid {get; private set;}

        private Dictionary<BattleshipsPosition, Ship> shipMap = new Dictionary<BattleshipsPosition, Ship>();
        
        public Board(int rows, int cols)
        {
            _rows = rows; _cols = cols;

            _grid = new Square[_rows, _cols];

            populateSquares();
        }

        private void populateSquares()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _grid[i,j] = new Square();
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

                    shipMap[new BattleshipsPosition(row, col)] = ship;
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

                    shipMap[new BattleshipsPosition(row, col)] = ship;
                }
            }
        }

        private bool SquareAlreadyHit(int row, int col)
        {
            return _grid[row,col].hit;
        }

        public bool Hit(BattleshipsPosition move)
        {
            if (SquareAlreadyHit(move.row, move.col)) return false;

            if(shipMap.TryGetValue(move, out var hitShip) && hitShip != null)
            {
                hitShip.hit();
            }

            _grid[move.row, move.col].hit = true;

            return true;
        }

        private bool IsOutsideBoundaries(int row, int col)
        {
            return row < 0 || row >= _rows || col < 0 || row >= _cols;
        }
    }
}