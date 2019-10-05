namespace GameHub.Games.BoardGames.Chess
{
    public class GameState
    {
        public string BoardStateFen { get; set; }

        public string Status { get; set; }

        public ChessPlayer CurrentTurnPlayer {get;set;}
    }
}