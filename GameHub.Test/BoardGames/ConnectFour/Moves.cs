using GameHub.Games.BoardGames.ConnectFour;
using Xunit;

namespace GameHub.Test.BoardGames.ConnectFourTests
{
    public class Moves
    {
        private static readonly string player1Id = "1234";
        private static readonly string player2Id = "abcd";
        private ConnectFour GetInitiatedGame()
        {
            var game = ConnectFourTestHelpers.GetGame(ConnectFourTestHelpers.GetDefaultConfig(player1Id));

            game.Join(player1Id, "user");
            game.Join(player2Id, "user2");

            return game;
        }

        [Fact]
        public void CanMove()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            var move = game.Move(player1Id, 1);

            Assert.True(move.WasSuccessful, "Failed to move despite all conditions being fine");
        }

        [Fact]
        public void CantMoveIfNotTurn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            var move = game.Move(player2Id, 1);

            Assert.False(move.WasSuccessful, "Player was able to move despite not being their turn");
        }

        // this checks to make sure the game keeps track of who's turn it is.
        [Fact]
        public void CantMoveTwiceInARow()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            var move1 = game.Move(player1Id, 1);
            var move2 = game.Move(player1Id, 1);

            Assert.True(move1.WasSuccessful, "Player was unable to move despite it being their turn");
            Assert.False(move2.WasSuccessful, "Player moved despite it not being their turn");
        }

        public void CantMoveOnInvalidColumn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            // Important to note that game is using the default configuration, which is 7 columns.
            var move = game.Move(player1Id, 100);

            Assert.False(move.WasSuccessful, "Was able to move on a column that does not exist");
        }

        [Fact]
        public void DetectsWinVertical()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            game.Move(player1Id, 1);
            game.Move(player2Id, 2);
            game.Move(player1Id, 1);
            game.Move(player2Id, 2);
            game.Move(player1Id, 1);
            game.Move(player2Id, 2);
            game.Move(player1Id, 1);

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }

        [Fact]
        public void DetectsWinHorizontal()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            game.Move(player1Id, 3);
            game.Move(player2Id, 2);
            game.Move(player1Id, 4);
            game.Move(player2Id, 2);
            game.Move(player1Id, 5);
            game.Move(player2Id, 2);
            game.Move(player1Id, 6);

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }

        [Fact]
        public void DetectsWinDiagonal()
        {
            // arrange
            var game = GetInitiatedGame();
            game.Start(player1Id);

            // act
            game.Move(player1Id, 1);
            game.Move(player2Id, 2);
            game.Move(player1Id, 2);
            game.Move(player2Id, 3);
            game.Move(player1Id, 4);
            game.Move(player2Id, 3);
            game.Move(player1Id, 4);
            game.Move(player2Id, 4);
            game.Move(player1Id, 3);
            game.Move(player2Id, 5);
            game.Move(player1Id, 4);
            

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }
    }
}