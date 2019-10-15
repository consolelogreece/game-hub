namespace GameHub.Games.BoardGames.Common
{
    public interface IBoardGame<TGameState, TGamePlayer> 
        where TGameState: GameState
        where TGamePlayer : GamePlayer
    {
        bool Resign(string playerId);
        bool Reset(string playerId);
        bool RegisterPlayer(string playerId, string playerNick);
        bool StartGame(string playerId);
        TGameState GetGameState();
        TGamePlayer GetPlayer(string playerId);
    }
}