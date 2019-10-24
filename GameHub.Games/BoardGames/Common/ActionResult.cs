namespace GameHub.Games.BoardGames.Common
{
    public class ActionResult
    {
        public bool WasSuccessful { get; private set; }

        public string Message { get; private set;}

        public ActionResult(bool wasSuccessful, string message = "")
        {
            WasSuccessful = wasSuccessful;
            Message = message;
        }
    }
}