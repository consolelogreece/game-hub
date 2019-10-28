using GameHub.Games.BoardGames.ConnectFour;
public class ConnectFourService
{
    private GameStarter _gameStarter;
    private GameJoiner _gameJoiner;
    private GamePlayerGetter _gamePlayerGetter;
    private GameResigner _gameResigner;
    private GameRestarter _gameRestarter;
    private GameStateGetter _gameStateGetter;
    
    public ConnectFourService(ConnectFour game)
    {
        _gameStarter = new GameStarter(game);

        _gameJoiner = new GameJoiner(game);

        _gamePlayerGetter = new GamePlayerGetter(game);

        _gameResigner = new GameResigner(game);

        _gameRestarter = new GameRestarter(game);

        _gameStateGetter = new GameStateGetter(game);
    }

    public void StartGame(string gameId, string playerId) => _gameStarter.StartGame(playerId);
}