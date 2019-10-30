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
    // id of player using the service
    private string _playerId;

    public ConnectFourService(ConnectFour game, string playerId)
    {
        _gameStarter = new GameStarter(game);

        _gameJoiner = new GameJoiner(game);

        _gameResigner = new GameResigner(game);

        _gameRestarter = new GameRestarter(game);

        _gamePlayerGetter = new GamePlayerGetter<ConnectFourPlayer>(game);

        _gameStateGetter = new GameStateGetter<GameStateConnectFour>(game);

        _gameMover = new GameMover<int>(game);

        _playerId = playerId;
    }

    public ActionResult StartGame() => _gameStarter.StartGame(_playerId);

    public ActionResult JoinGame(string playerNick) => _gameJoiner.JoinGame(_playerId, playerNick);

    public ConnectFourPlayer GetPlayer() => (ConnectFourPlayer)_gamePlayerGetter.Get(_playerId);

    public ActionResult Resign() => _gameResigner.Resign(_playerId);

    public ActionResult Restart() => _gameRestarter.Restart(_playerId);

    public GameStateConnectFour GetGameState() => (GameStateConnectFour)_gameStateGetter.Get();

    public ActionResult Move(int col) => _gameMover.Move(_playerId, col);
}