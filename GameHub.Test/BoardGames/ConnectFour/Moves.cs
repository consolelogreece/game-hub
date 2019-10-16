using GameHub.Games.BoardGames.ConnectFour;
using Xunit;

namespace GameHub.Test.BoardGames
{
    public class Moves
    {
        private static readonly string player1Id = "1234";
        private static readonly string player2Id = "abcd";
        private ConnectFour GetInitiatedGame()
        {
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig(player1Id));

            game.RegisterPlayer(player1Id, "user");
            game.RegisterPlayer(player2Id, "user");

            return game;
        }

        [Fact]
        public void CanMove()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var movedSuccessfully = game.MakeMove(1, player1Id);

            Assert.True(movedSuccessfully, "Failed to move despite all conditions being fine");
        }

        [Fact]
        public void CantMoveIfNotTurn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var movedSuccessfully = game.MakeMove(1, player2Id);

            Assert.False(movedSuccessfully, "Player was able to move despite not being their turn");
        }

        // this checks to make sure the game keeps track of who's turn it is.
        [Fact]
        public void CantMoveTwiceInARow()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var move1Successful = game.MakeMove(1, player1Id);
            var move2Successful = game.MakeMove(1, player1Id);

            Assert.True(move1Successful, "Player was unable to move despite it being their turn");
            Assert.False(move2Successful, "Player moved despite it not being their turn");
        }

        public void CantMoveOnInvalidColumn()
        {
                // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            // Important to node that game is using the default configuration, which is 7 columns.
            var moveSuccessful = game.MakeMove(100, player1Id);

            Assert.False(moveSuccessful, "Was able to move on a column that does not exist");
        }
    }
}