using GameHub.Games.BoardGames.Common;

public class GameRestarter
{
    private IRestartable _game;
    
    public GameRestarter(IRestartable game)
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