using GameHub.Games.BoardGames.Common;

public class GameStarter
{
    private IStartable _game;

    public GameStarter(IStartable game)
    {
        _game = game;
    }

    public ActionResult StartGame(string playerId)
    {
        lock(_game)
        {
            return _game.Start(playerId);
        }
    }
}