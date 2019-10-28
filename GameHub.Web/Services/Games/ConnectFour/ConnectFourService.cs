using GameHub.Games.BoardGames.Common;
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

    public ActionResult StartGame(string playerId) => _gameStarter.StartGame(playerId);

    public ActionResult JoinGame(string playerId, string playerNick) => _gameJoiner.JoinGame(playerId, playerNick);

    public ConnectFourPlayer GetPlayer(string playerId) => (ConnectFourPlayer)_gamePlayerGetter.Get(playerId);

    public ActionResult Resign(string playerId) => _gameResigner.Resign(playerId);
ActionResult
    public ActionResult Restart(string playerId) => _gameRestarter.Restart(playerId);

    public GameStateConnectFour GetGameState() => (GameStateConnectFour)_gameStateGetter.Get();


}