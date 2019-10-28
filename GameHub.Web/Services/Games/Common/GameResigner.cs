using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

public class GameResigner
{
    private ConnectFour _game;

    look // change to IResignable
    public GameResigner(ConnectFour game)
    {
        _game = game;
    }
    
    public ActionResult Resign(string playerId)
    {
        lock(_game)
        {
            return _game.StartGame(playerId);
        }
    }
}