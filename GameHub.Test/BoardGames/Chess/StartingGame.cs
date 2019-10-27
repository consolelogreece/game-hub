using Xunit;

namespace GameHub.Test.BoardGames.ChessTests
{
    public class StartingGame
    {
        [Fact]
        public void OnlyHostCanStartGame()
        {
            // arrange
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            game.RegisterPlayer("abcd", "user2");

            var regularPlayerStartResult = game.StartGame("abcd");
            var hostStartResult = game.StartGame("1234");
        
            // assert
            Assert.False(regularPlayerStartResult.WasSuccessful, "Non host player was able to start the game");
            Assert.True(hostStartResult.WasSuccessful, "Host was unable to start the game");
        }

        [Fact]
        public void CantStartGameWithoutAtleast2Players()
        {
            // arrange
           var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            
            var startGameWithOnePlayerResult = game.StartGame("1234");

            game.RegisterPlayer("abcd", "user2");

            var startStartGameWithTwoPlayersResult = game.StartGame("1234");

            // assert
            Assert.False(startGameWithOnePlayerResult.WasSuccessful, "Was able to start game with only 1 player registered");
            Assert.True(startStartGameWithTwoPlayersResult.WasSuccessful, "Was unable to start game with 2 players registered");
        }
    }
}