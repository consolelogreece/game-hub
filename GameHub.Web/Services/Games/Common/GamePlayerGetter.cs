using GameHub.Games.BoardGames.Common;

public class GamePlayerGetter
{
    private IGamePlayerGetter _game;

    public GamePlayerGetter(IGamePlayerGetter game)
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