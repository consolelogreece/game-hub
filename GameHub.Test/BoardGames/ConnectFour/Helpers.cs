using GameHub.Games.BoardGames.ConnectFour;

namespace GameHub.Test.BoardGames.ConnectFourTests
{
    internal static class ConnectFourTestHelpers
    {
        internal static ConnectFour GetGame()
        {
            return GetGame(new ConnectFourConfiguration());
        }
        internal static ConnectFour GetGame(ConnectFourConfiguration config)
        {
            return new ConnectFour(config);
        }

        internal static ConnectFourConfiguration GetDefaultConfig(string creatorId)
        {
            return new ConnectFourConfiguration
            {
                creatorId = creatorId,
                nCols = 7,
                nRows = 6,
                winThreshold = 4,
                nPlayersMax = 2
            };
        }
    }
}