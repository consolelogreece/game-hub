using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Chess
{
    public class ChessPlayer : GamePlayer
    {
        public ChessDotNet.Player player { get; set; }
    }
}