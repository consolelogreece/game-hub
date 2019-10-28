using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Web.Services.Games.Common;

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