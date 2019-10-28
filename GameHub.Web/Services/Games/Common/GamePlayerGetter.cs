using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

public class GamePlayerGetter
{
   private ConnectFour _game;

    look  // change from ConnectFour to IPlayerGettable
    public GamePlayerGetter(ConnectFour game)
    {
        _game = game;
    }
    
    public GamePlayer Get(string playerId)
    {
        lock(_game)
        {
            return _game.GetPlayer(playerId);
        }
    }
}