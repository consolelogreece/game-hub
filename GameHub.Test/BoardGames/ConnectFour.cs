using GameHub.Games.BoardGames.ConnectFour;
using Xunit;
// cant register more users than max
// resign functionality
// rematch functionalty

namespace GameHub.Test.BoardGames
{
    public class ConnectFourTests
    {
        private static ConnectFour GetGame()
        {
            return GetGame(new ConnectFourConfiguration());
        }
        private static ConnectFour GetGame(ConnectFourConfiguration config)
        {
            return new ConnectFour(config);
        }

        private static ConnectFourConfiguration GetDefaultConfig(string creatorId)
        {
            return new ConnectFourConfiguration
            {
                creatorId = creatorId,
                nCols = 7,
                nRows = 6,
                winThreshold = 4,
                nPlayersMax = 2
            };
        }

        public class PlayerRegistration
        {
            [Fact]
            public void CanRegisterPlayer()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));

                // act
                var isRegistrationComplete = game.RegisterPlayer("1234", "user");

                // assert
                Assert.True(isRegistrationComplete, "Failed to register player");
            }

            [Fact]
            public void CantRegisterPlayerOnceGameHasStarted()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));

                // act
                game.RegisterPlayer("1234", "user");
                game.RegisterPlayer("abcd", "user2");

                game.StartGame("1234");

                var isRegistrationSuccessful = game.RegisterPlayer("zxcv", "user3");

                // assert
                Assert.False(isRegistrationSuccessful, "Was able to register player after game has already started");
            }

            [Fact]
            public void CanGetPlayer()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));

                // act
                game.RegisterPlayer("1234", "user");

                var player = game.GetPlayer("1234");

                // assert
                Assert.NotNull(player);
            }

            [Fact]
            public void GetsCorrectPlayer()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));

                // act
                game.RegisterPlayer("1234", "user");
                game.RegisterPlayer("abcd", "user2");

                var player = game.GetPlayer("1234");

                // assert
                Assert.Equal(player.Id, "1234");
            }

            [Fact]
            public void SetsHostCorrectly()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));
                
                // act
                game.RegisterPlayer("1234", "user");
                game.RegisterPlayer("abcd", "user2");

                var playerHost = game.GetPlayer("1234");
                var playerNormal = game.GetPlayer("abcd");

                // assert
                Assert.True(playerHost.IsHost, "Not setting host flag to true on host player");
                Assert.False(playerNormal.IsHost, "Incorrectly setting host flag to true on normal player");
            }
        }

        public class StartingGame
        {
            [Fact]
            public void OnlyHostCanStartGame()
            {
                // arrange
                var game = GetGame(GetDefaultConfig("1234"));

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
                var game = GetGame(GetDefaultConfig("1234"));

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

        public class Moves
        {
            private static readonly string player1Id = "1234";
            private static readonly string player2Id = "abcd";
            private ConnectFour GetInitiatedGame()
            {
                var game = GetGame(GetDefaultConfig(player1Id));

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
}