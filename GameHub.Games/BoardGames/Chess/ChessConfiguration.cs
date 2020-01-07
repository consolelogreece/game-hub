using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Chess
{
    public class ChessConfiguration : GameConfiguration
    {
        public int nPlayersMax { get; private set; } = 2;
    }  
}