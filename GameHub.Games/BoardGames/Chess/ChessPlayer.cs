using ChessDotNet;

namespace GameHub.Games.BoardGames.Chess
{
    public class ChessPlayer
    {
        public string Id { get; set; }

        public bool IsHost {get; set;}

        public string PlayerNick { get; set; }

        public Player player { get; set; }
    }
}
