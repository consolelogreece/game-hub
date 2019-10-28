using GameHub.Games.BoardGames.Common;

public class GamePlayerGetter<T> where T : GamePlayer
{
    private IGamePlayerGetter<T> _game;

    public GamePlayerGetter(IGamePlayerGetter<T> game)
    {
        _game = game;
    }
    
    public T Get(string playerId)
    {
        lock(_game)
        {
            return _game.GetPlayer(playerId);
        }
    }
}