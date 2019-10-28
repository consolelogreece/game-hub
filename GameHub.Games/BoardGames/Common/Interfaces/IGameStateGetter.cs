namespace GameHub.Games.BoardGames.Common
{
    public interface IGameStateGetter<T> where T : GameState
    {   
        T GetGameState();
    }
}