namespace GameHub.Games.BoardGames.Common
{
    public class MoveResult
    {
        public bool WasValid { get; private set; }

        public string Message { get; private set;}

        public MoveResult(bool wasValid, string message = "")
        {
            WasValid = wasValid;
            Message = message;
        }
    }
}