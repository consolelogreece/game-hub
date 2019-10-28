using GameHub.Games.BoardGames.Common;
using GameHub.Web.Services.Games.Common;

public class GameResigner
{
    private IResignable _game;
    public GameResigner(IResignable game)
    {
        _game = game;
    }
    
    public ActionResult Resign(string playerId)
    {
        lock(_game)
        {
            return _game.Resign(playerId);
        }
    }
}