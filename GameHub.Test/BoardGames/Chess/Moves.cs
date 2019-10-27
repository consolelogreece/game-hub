using GameHub.Games.BoardGames.Chess;
using ChessDotNet;
using Xunit;

namespace GameHub.Test.BoardGames.ChessTests
{
    public class Moves
    {
        private static readonly string player1Id = "1234";
        private static readonly string player2Id = "abcd";
        private Chess GetInitiatedGame()
        {
            var game = ChessTestHelpers.GetGame(ChessTestHelpers.GetDefaultConfig(player1Id));

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
            var move = new ChessDotNet.Move("e2", "e4", Player.White);

            // act
            var moveWasSuccessful = game.MakeMove(player1Id, move);

            // assert
            Assert.True(moveWasSuccessful, "Failed to move despite all conditions being fine");
        }

        [Fact]
        public void CantMoveIfNotTurn()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);
            var move = new ChessDotNet.Move("e7", "e5", Player.Black);

            // act
            var moveWasSuccessful = game.MakeMove(player2Id, move);

            Assert.False(moveWasSuccessful, "Player was able to move despite not being their turn");
        }

        // this checks to make sure the game keeps track of who's turn it is.
        [Fact]
        public void CantMoveTwiceInARow()
        {
            // arrange
            var game = GetInitiatedGame();
            game.StartGame(player1Id);
            var move1 = new ChessDotNet.Move("e2", "e4", Player.White);
            var move2 = new ChessDotNet.Move("f2", "f4", Player.White);

            // act
            var move1Successful = game.MakeMove(player1Id, move1);
            var move2Successful = game.MakeMove(player1Id, move2);

            Assert.True(move1Successful, "Player was unable to move despite it being their turn");
            Assert.False(move2Successful, "Player moved despite it not being their turn");
        }
    }
}