using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;
public class ConnectFourService
{
    private GameStarter _gameStarter;
    private GameJoiner _gameJoiner;
    private GameResigner _gameResigner;
    private GameRestarter _gameRestarter;
    private GamePlayerGetter<ConnectFourPlayer> _gamePlayerGetter;
    private GameStateGetter<GameStateConnectFour> _gameStateGetter;
    private GameMover<int> _gameMover;

    public ConnectFourService(ConnectFour game)
    {
        _gameStarter = new GameStarter(game);

        _gameJoiner = new GameJoiner(game);

        _gameResigner = new GameResigner(game);

        _gameRestarter = new GameRestarter(game);

        _gamePlayerGetter = new GamePlayerGetter<ConnectFourPlayer>(game);

        _gameStateGetter = new GameStateGetter<GameStateConnectFour>(game);

        _gameMover = new GameMover<int>(game);
    }

    public ActionResult StartGame(string playerId) => _gameStarter.StartGame(playerId);

    public ActionResult JoinGame(string playerId, string playerNick) => _gameJoiner.JoinGame(playerId, playerNick);

    public ConnectFourPlayer GetPlayer(string playerId) => (ConnectFourPlayer)_gamePlayerGetter.Get(playerId);

    public ActionResult Resign(string playerId) => _gameResigner.Resign(playerId);

    public ActionResult Restart(string playerId) => _gameRestarter.Restart(playerId);

    public GameStateConnectFour GetGameState() => (GameStateConnectFour)_gameStateGetter.Get();

    public ActionResult Move(string playerId, int col) => _gameMover.Move(playerId, col);
}