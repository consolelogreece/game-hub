using GameHub.Games.BoardGames.Common;

public class GameJoiner
{
    private IJoinable _game;

    public GameJoiner(IJoinable game)
    {
        _game = game;
    }
    
    public ActionResult JoinGame(string playerId, string playerNick)
    {
        lock(_game)
        {
            return _game.Join(playerId, playerNick);
        }
    }
}