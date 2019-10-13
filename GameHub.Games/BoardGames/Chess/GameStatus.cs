namespace GameHub.Games.BoardGames.Chess
{
    public class GameStatus
    {
        public GameStatus(string status = "", string endReason = "")
        {
            Status = status;

            EndReason = endReason;
        }
        
        public string Status { get; private set; }

        public string EndReason { get; private set; }
    }
}