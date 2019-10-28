using GameHub.Games.BoardGames.Common;
using GameHub.Web.Services.Games.Common;

public class GameRematcher
{
    private IRestartable _game;
    
    public GameRematcher(IRestartable game)
    {
        _game = game;
    }
    
    public ActionResult Restart(string playerId)
    {
        lock(_game)
        {
            return _game.Restart(playerId);
        }
    }
}