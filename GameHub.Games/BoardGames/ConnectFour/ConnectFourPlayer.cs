using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourPlayer : GamePlayer
    {
        public string PlayerColor { get; set; }

        public bool Resigned { get; set; }
    }
}
