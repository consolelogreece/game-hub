using GameHub.Games.BoardGames.Chess;

namespace GameHub.Test.BoardGames.ChessTests
{
    internal static class ChessTestHelpers
    {
        internal static Chess GetGame()
        {
            return GetGame(new ChessConfig());
        }
        internal static Chess GetGame(ChessConfig config)
        {
            return new Chess(config);
        }

        internal static ChessConfig GetDefaultConfig(string creatorId)
        {
            return new ChessConfig
            {
                creatorId = creatorId
            };
        }
    }
}