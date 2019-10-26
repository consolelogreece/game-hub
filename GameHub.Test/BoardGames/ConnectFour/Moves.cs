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

            game.RegisterPlayer(player1Id, "user");
            game.RegisterPlayer(player2Id, "user2");

            return game;
        }

        [Fact]
        public void CanMove()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var move = game.MakeMove(1, player1Id);

            Assert.True(move.WasSuccessful, "Failed to move despite all conditions being fine");
        }

        [Fact]
        public void CantMoveIfNotTurn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var move = game.MakeMove(1, player2Id);

            Assert.False(move.WasSuccessful, "Player was able to move despite not being their turn");
        }

        // this checks to make sure the game keeps track of who's turn it is.
        [Fact]
        public void CantMoveTwiceInARow()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            var move1 = game.MakeMove(1, player1Id);
            var move2 = game.MakeMove(1, player1Id);

            Assert.True(move1.WasSuccessful, "Player was unable to move despite it being their turn");
            Assert.False(move2.WasSuccessful, "Player moved despite it not being their turn");
        }

        public void CantMoveOnInvalidColumn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            // Important to note that game is using the default configuration, which is 7 columns.
            var move = game.MakeMove(100, player1Id);

            Assert.False(move.WasSuccessful, "Was able to move on a column that does not exist");
        }

        [Fact]
        public void DetectsWinVertical()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            game.MakeMove(1, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(1, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(1, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(1, player1Id);

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }


        [Fact]
        public void DetectsWinHorizontal()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            game.MakeMove(3, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(4, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(5, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(6, player1Id);

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }

        [Fact]
        public void DetectsWinDiagonal()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);

            // act
            game.MakeMove(1, player1Id);
            game.MakeMove(2, player2Id);
            game.MakeMove(2, player1Id);
            game.MakeMove(3, player2Id);
            game.MakeMove(4, player1Id);
            game.MakeMove(3, player2Id);
            game.MakeMove(4, player1Id);
            game.MakeMove(4, player2Id);
            game.MakeMove(3, player1Id);
            game.MakeMove(5, player2Id);
            game.MakeMove(4, player1Id);
            

            var gameState = game.GetGameState();

            Assert.Equal(gameState.Status.Status,"finished");
        }
    }
}