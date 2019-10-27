namespace GameHub.Games.BoardGames.Chess
{
    public class ChessConfiguration
    {
        public string creatorId {get; set;}

        public ushort nPlayersMax { get; private set; } = 2;
    }  
}