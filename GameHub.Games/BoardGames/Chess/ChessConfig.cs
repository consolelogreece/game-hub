namespace GameHub.Games.BoardGames.Chess
{
    public class ChessConfig
    {
        public string creatorId {get; set;}

        public ushort nPlayersMax { get; private set; } = 2;
    }  
}