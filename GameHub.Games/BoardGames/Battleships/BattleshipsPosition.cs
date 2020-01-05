namespace GameHub.Games.BoardGames.Battleships
{
    public struct BattleshipsPosition
    {
        public BattleshipsPosition(int Row, int Col)
        {
            row = Row;
            col = Col;
        }
        public static bool operator ==(BattleshipsPosition pos1, BattleshipsPosition pos2)
        {
            return pos1.col == pos2.col && pos1.row == pos2.row;
        }

        public static bool operator !=(BattleshipsPosition pos1, BattleshipsPosition pos2)
        {
            return pos1.col != pos2.col || pos1.row != pos2.row;
        }
        
        public override bool Equals(object obj)
        {
            var pos = (BattleshipsPosition)obj;
            return pos == this;
        }

        public override int GetHashCode()
        {
            return row + (col + (((row + 1)/2) * ((row + 1)/2)));
        }

        public override string ToString()
        {
            return row + "," + col;
        }

        public int row;

        public int col;

    }
}
