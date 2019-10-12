namespace GameHub.Games.BoardGames.Chess
{
    public class GameStatus
    {
        public GameStatus(string status = "", ChessPlayer winner = null)
        {
            Status = status;

            Winner = winner;
        }
        
        public string Status { get; private set; }

        public ChessPlayer Winner { get; private set; }
    }
}