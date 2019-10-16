using Xunit;

namespace GameHub.Test.BoardGames
{
    public class PlayerRegistration
    {
        [Fact]
        public void CanRegisterPlayer()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            var isRegistrationComplete = game.RegisterPlayer("1234", "user");

            // assert
            Assert.True(isRegistrationComplete, "Failed to register player");
        }

        [Fact]
        public void CantRegisterPlayerOnceGameHasStarted()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            game.StartGame("1234");

            var isRegistrationSuccessful = game.RegisterPlayer("zxcv", "user3");

            // assert
            Assert.False(isRegistrationSuccessful, "Was able to register player after game has already started");
        }

        [Fact]
        public void CantRegisterMoreThanMaxPlayers()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            var isRegistrationSuccessful = game.RegisterPlayer("zxcv", "user3");

            // assert
            Assert.False(isRegistrationSuccessful, "Was able to register player despite game being full");
        }

        [Fact]
        public void CantRegisterSameIDTwice()
        {
          // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            
            var isRegistrationSuccessful = game.RegisterPlayer("1234", "user2");

            // assert
            Assert.False(isRegistrationSuccessful, "Was able to register player ID already being in use");  
        }

        [Fact]
        public void CanGetPlayer()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

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
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

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
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));
            
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