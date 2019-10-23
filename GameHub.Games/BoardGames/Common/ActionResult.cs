namespace GameHub.Games.BoardGames.Common
{
    public class ActionResult
    {
        public bool WasValid { get; private set; }

        public string Message { get; private set;}

        public ActionResult(bool wasValid, string message = "")
        {
            WasValid = wasValid;
            Message = message;
        }
    }
}