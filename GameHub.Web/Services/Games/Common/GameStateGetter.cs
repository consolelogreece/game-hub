using GameHub.Games.BoardGames.Common;

public class GameStateGetter<T> where T : GameState
{
    private IGameStateGetter<T> _game;

    public GameStateGetter(IGameStateGetter<T> game)
    {
        _game = game;
    }
    
    public T Get()
    {
        lock(_game)
        {
            return _game.GetGameState();
        }
    }
}