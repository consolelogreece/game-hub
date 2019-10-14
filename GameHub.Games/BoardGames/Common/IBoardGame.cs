namespace GameHub.Games.BoardGames.Common
{
    public interface IBoardGame
    {
        bool Resign(string playerId);
        bool Reset(string playerId);
        bool RegisterPlayer(string playerId, string playerNick);
        bool StartGame();
        GameStateBase GetGameState();
    }
}