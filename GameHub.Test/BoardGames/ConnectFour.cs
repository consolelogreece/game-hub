using GameHub.Games.BoardGames.ConnectFour;
using Xunit;
// cant register more users than max
// resign functionality
// rematch functionalty

namespace GameHub.Test.BoardGames
{
    public class ConnectFourTests
    {
        private ConnectFour GetGame()
        {
            return GetGame(new ConnectFourConfiguration());
        }
        private ConnectFour GetGame(ConnectFourConfiguration config)
        {
            return new ConnectFour(config);
        }

        private ConnectFourConfiguration GetDefaultConfig(string creatorId)
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

        [Fact]
        public void CanRegisterPlayer()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            var isRegistrationComplete = game.RegisterPlayer("1234", "user");

            // assert
            Assert.True(isRegistrationComplete, "Failed to register player");
        }

        [Fact]
        public void OnlyHostCanStartGame()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            var canRegularPlayerStart = game.StartGame("abcd");
            var canHostStart = game.StartGame("1234");
        
            // assert
            Assert.False(canRegularPlayerStart, "Non host player was able to start the game");
            Assert.True(canHostStart, "Host was unable to start the game");
        }

        [Fact]
        public void CantStartGameWithoutAtleast2Players()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            
            var canStartGameWithOnePlayer = game.StartGame("1234");

            game.RegisterPlayer("abcd", "user2");

            var canStartGameWithTwoPlayers = game.StartGame("1234");

            // assert
            Assert.False(canStartGameWithOnePlayer, "Was able to start game with only 1 player registered");
            Assert.True(canStartGameWithTwoPlayers, "Was unable to start game with 2 players registered");
        }

        [Fact]
        public void CantRegisterPlayerOnceGameHasStarted()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            game.StartGame("1234");

            var isRegistrationSuccessful = game.RegisterPlayer("zxcv", "user3");

            // assert
            Assert.False(isRegistrationSuccessful, "Was able to register player after game has already started");
        }

        [Fact]
        public void CanGetPlayer()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");

            var player = game.GetPlayer("1234");

            // assert
            Assert.NotNull(player);
        }

        [Fact]
        public void GetsCorrectPlayer()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            var player = game.GetPlayer("1234");

            // assert
            Assert.Equal(player.Id, "1234");
        }

        [Fact]
        public void SetsHostCorrectly()
        {
            // arrange
            var game = GetGame(GetDefaultConfig("1234"));
            
            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            var playerHost = game.GetPlayer("1234");
            var playerNormal = game.GetPlayer("abcd");

            // assert
            Assert.True(playerHost.IsHost, "Not setting host flag to true on host player");
            Assert.False(playerNormal.IsHost, "Incorrectly setting host flag to true on normal player");
        }
    }
}