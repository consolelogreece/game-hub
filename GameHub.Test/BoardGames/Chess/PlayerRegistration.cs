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
            var registrationResult = game.RegisterPlayer("1234", "user");

            // assert
            Assert.True(registrationResult.WasSuccessful, "Failed to register player");
        }

        [Fact]
        public void CantRegisterPlayerOnceGameHasStarted()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            game.StartGame("1234");

            var registrationResult = game.RegisterPlayer("zxcv", "user3");

            // assert
             Assert.False(registrationResult.WasSuccessful, "Was able to register player after game has already started");
        }  
    }
}