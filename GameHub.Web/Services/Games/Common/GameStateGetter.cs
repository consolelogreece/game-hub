using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

public class GameStateGetter
{
   private ConnectFour _game;

    look  // change from ConnectFour to IPlayerGettable
    public GameStateGetter(ConnectFour game)
    {
        _game = game;
    }
    
    public GameState Get()
    {
        lock(_game)
        {
            return _game.GetGameState();
        }
    }
}