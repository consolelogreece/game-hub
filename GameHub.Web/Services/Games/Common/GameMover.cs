using GameHub.Games.BoardGames.Common;

public class GameMover
{
    private IMoveable _game;

    public GameMover(IMoveable game)
    {
        _game = game;
    }
    
    public ActionResult Move<T>(string playerId, T move)
    {
        lock(_game)
        {
            return _game.Move(playerId, move);
        }
    }
}