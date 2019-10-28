using GameHub.Games.BoardGames.ConnectFour;
public class ConnectFourService
{
    private GameStarter _gameStarter;
    public ConnectFourService(ConnectFour game)
    {
        _gameStarter = new GameStarter(game);
    }

    public void StartGame(string gameId, string playerId) => _gameStarter.StartGame(playerId);
}