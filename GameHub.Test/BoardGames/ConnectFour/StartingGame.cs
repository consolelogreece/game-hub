using Xunit;

namespace GameHub.Test.BoardGames.ConnectFourTests
{
    public class StartingGame
    {
        [Fact]
        public void OnlyHostCanStart()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            game.Join("abcd", "user2");

            var regularPlayerStartResult = game.Start("abcd");
            var hostStartResult = game.Start("1234");
        
            // assert
            Assert.False(regularPlayerStartResult.WasSuccessful, "Non host player was able to start the game");
            Assert.True(hostStartResult.WasSuccessful, "Host was unable to start the game");
        }

        [Fact]
        public void CantStartWithoutAtleast2Players()
        {
            // arrange
           var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.Join("1234", "user");
            
            var StartWithOnePlayerResult = game.Start("1234");

            game.Join("abcd", "user2");

            var startStartWithTwoPlayersResult = game.Start("1234");

            // assert
            Assert.False(StartWithOnePlayerResult.WasSuccessful, "Was able to start game with only 1 player registered");
            Assert.True(startStartWithTwoPlayersResult.WasSuccessful, "Was unable to start game with 2 players registered");
        }
    }
}