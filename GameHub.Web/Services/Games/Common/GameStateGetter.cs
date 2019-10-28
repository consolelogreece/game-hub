using GameHub.Games.BoardGames.Common;

public class GameStateGetter
{
    private IGameStateGetter _game;

    public GameStateGetter(IGameStateGetter game)
    {
        _game = game;
    }
    
    public GameState Get()
    {
        lock(_game)
        {
            return _game.GetState();
        }
    }
}