using GameHub.Games.BoardGames.Chess;

namespace GameHub.Test.BoardGames.ChessTests
{
    internal static class ChessTestHelpers
    {
        internal static Chess GetGame()
        {
            return GetGame(new ChessConfiguration());
        }
        internal static Chess GetGame(ChessConfiguration config)
        {
            return new Chess(config);
        }

        internal static ChessConfiguration GetDefaultConfig(string creatorId)
        {
            return new ChessConfiguration
            {
                creatorId = creatorId
            };
        }
    }
}