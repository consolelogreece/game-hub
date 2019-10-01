using ChessDotNet;

namespace GameHub.Games.BoardGames.Chess
{
    public class ChessMove
    {
        public string From { get; set; }

        public string To { get; set; }

        public char? Promotion { get; set; }
    }
}