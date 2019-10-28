using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

public class GameRematcher
{
   private ConnectFour _game;

    look  // change from ConnectFour to IRestartable
    public GameRematcher(ConnectFour game)
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