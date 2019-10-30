using Xunit;

namespace GameHub.Test.BoardGames.ChessTests
{
    public class Resignation
    {
        [Fact]
        public void CanResign()
        {
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            game.Join("1234", "player1");
            game.Join("abcd", "player2");

            game.Start("1234");

            var resignResult = game.Resign("1234");

            Assert.True(resignResult.WasSuccessful);
        }

        [Fact]
        public void CantResignIfGameNotStarted()
        {
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            game.Join("1234", "player1");
            game.Join("abcd", "player2");

            var resignResult = game.Resign("1234");

            Assert.False(resignResult.WasSuccessful);
        }


        [Fact]
        public void CantResignTwice()
        {
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig("1234"));

            game.Join("1234", "player1");
            game.Join("abcd", "player2");

            game.Start("1234");

            var resignResult1 = game.Resign("1234");
            var resignResult2 = game.Resign("1234");

            Assert.True(resignResult1.WasSuccessful);
            Assert.False(resignResult2.WasSuccessful);
        }
    }
}