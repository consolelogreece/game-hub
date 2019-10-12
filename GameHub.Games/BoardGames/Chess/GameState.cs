namespace GameHub.Games.BoardGames.Chess
{
    public class GameState
    {
        public string BoardStateFen { get; set; }

        public ChessPlayer CurrentTurnPlayer {get;set;}

        public GameStatus Status { get; set; }
    }
}