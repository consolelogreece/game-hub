namespace GameHub.Games.BoardGames.Battleships
{
    public class Ship : ShipModel
    {
        private int hits = 0;

        public Ship(ShipModel model)
        {
            this.Id = model.Id;
            this.col = model.col;
            this.row = model.row;
            this.orientation = model.orientation;
            this.length = model.length;
        }

        public void hit()
        {
            hits++;
        }

        public bool IsSunk()
        {
            return length <= hits;
        }
    }
}