namespace GameHub.Games.BoardGames.Common
{
    public interface IBoardGame<out TGameState, out TGamePlayer> 
        where TGameState: GameState
        where TGamePlayer : GamePlayer
    {
        ActionResult Resign(string playerId);
        ActionResult Reset(string playerId);
        ActionResult RegisterPlayer(string playerId, string playerNick);
        ActionResult StartGame(string playerId);
        TGameState GetGameState();
        TGamePlayer GetPlayer(string playerId);
    }
}