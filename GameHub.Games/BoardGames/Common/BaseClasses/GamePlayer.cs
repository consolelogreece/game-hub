namespace GameHub.Games.BoardGames.Common
{
    public class GamePlayer
    {
        public string Id { get; set; }

        public bool IsHost {get; set;}

        public string PlayerNick { get; set; }

        public int Wins { get; set; }
    }
}