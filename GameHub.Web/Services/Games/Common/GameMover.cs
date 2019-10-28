using GameHub.Games.BoardGames.Common;

public class GameMover<T>
{
    private IMoveable<T> _game;

    public GameMover(IMoveable<T> game)
    {
        _game = game;
    }
    
    public ActionResult Move(string playerId, T move)
    {
        lock(_game)
        {
            return _game.Move(playerId, move);
        }
    }
}