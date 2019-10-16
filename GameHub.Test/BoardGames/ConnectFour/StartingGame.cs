using Xunit;

namespace GameHub.Test.BoardGames
{
    public class StartingGame
    {
        [Fact]
        public void OnlyHostCanStartGame()
        {
            // arrange
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

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
           var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            // act
            game.RegisterPlayer("1234", "user");
            
            var canStartGameWithOnePlayer = game.StartGame("1234");

            game.RegisterPlayer("abcd", "user2");

            var canStartGameWithTwoPlayers = game.StartGame("1234");

            // assert
            Assert.False(canStartGameWithOnePlayer, "Was able to start game with only 1 player registered");
            Assert.True(canStartGameWithTwoPlayers, "Was unable to start game with 2 players registered");
        }
    }
}