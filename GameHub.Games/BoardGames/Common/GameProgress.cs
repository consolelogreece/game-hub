namespace GameHub.Games.BoardGames.Common
{
    public class GameProgress
    {
        public GameProgress(string status = "", string endReason = "")
        {
            Status = status;

            EndReason = endReason;
        }
        
        public string Status { get; private set; }

        public string EndReason { get; private set; }
    }
}