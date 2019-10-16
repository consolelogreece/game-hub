using Xunit;

namespace GameHub.Test.BoardGames
{
    public class Resignation
    {
        [Fact]
        public void CanResign()
        {
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            game.RegisterPlayer("1234", "player1");
            game.RegisterPlayer("abcd", "player2");

            game.StartGame("1234");

            var resignedSuccessfully = game.Resign("1234");

            Assert.True(resignedSuccessfully);
        }

        [Fact]
        public void CantResignIfGameNotStarted()
        {
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            game.RegisterPlayer("1234", "player1");
            game.RegisterPlayer("abcd", "player2");

            var resignedSuccessfully = game.Resign("1234");

            Assert.False(resignedSuccessfully);
        }


        [Fact]
        public void CantResignTwice()
        {
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig("1234"));

            game.RegisterPlayer("1234", "player1");
            game.RegisterPlayer("abcd", "player2");

            game.StartGame("1234");

            var resignedSuccessfully1 = game.Resign("1234");
            var resignedSuccessfully2 = game.Resign("1234");

            Assert.True(resignedSuccessfully1);
            Assert.False(resignedSuccessfully2);
        }
    }
}