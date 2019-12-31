namespace GameHub.Games.BoardGames.Battleships
{
    public struct BattleshipsPosition
    {
        public BattleshipsPosition(int Row, int Col)
        {
            row = Row;
            col = Col;
        }
        
        public int row;

        public int col;
    }
}
