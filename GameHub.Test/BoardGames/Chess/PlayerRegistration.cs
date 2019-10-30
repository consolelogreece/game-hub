using Xunit;

namespace GameHub.Test.BoardGames.ChessTests
{
    public class PlayerRegistration
    {
        [Fact]
        public void CanRegisterPlayer()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            var registrationResult = game.Join("1234", "user");

            // assert
            Assert.True(registrationResult.WasSuccessful, "Failed to register player");
        }

        [Fact]
        public void CantRegisterPlayerOnceGameHasStarted()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            game.Join("abcd", "user2");

            game.Start("1234");

            var registrationResult = game.Join("zxcv", "user3");

            // assert
             Assert.False(registrationResult.WasSuccessful, "Was able to register player after game has already started");
        }  


        public void CantRegisterMoreThanMaxPlayers()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            game.Join("abcd", "user2");

            var registrationResult = game.Join("zxcv", "user3");

            // assert
            Assert.False(registrationResult.WasSuccessful, "Was able to register player despite game being full");
        }

        [Fact]
        public void CantRegisterSameIDTwice()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            
            var registrationResult = game.Join("1234", "user2");

            // assert
            Assert.False(registrationResult.WasSuccessful, "Was able to register player ID already being in use");  
        }

        [Fact]
        public void CantRegisterSameNickTwice()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            
            var registrationResult = game.Join("1234", "user");

            // assert
            Assert.False(registrationResult.WasSuccessful, "Was able to register player nick which is already in use");  
        }

        [Fact]
        public void CanGetPlayer()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");

            var player = game.GetPlayer("1234");

            // assert
            Assert.NotNull(player);
        }

        [Fact]
        public void GetsCorrectPlayer()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            game.Join("abcd", "user2");

            var player = game.GetPlayer("1234");

            // assert
            Assert.Equal(player.Id, "1234");
        }

        [Fact]
        public void SetsHostCorrectly()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));
            
            // act
            game.Join("1234", "user");
            game.Join("abcd", "user2");

            var playerHost = game.GetPlayer("1234");
            var playerNormal = game.GetPlayer("abcd");

            // assert
            Assert.True(playerHost.IsHost, "Not setting host flag to true on host player");
            Assert.False(playerNormal.IsHost, "Incorrectly setting host flag to true on normal player");
        }
    }
}