using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Common
{
    public interface IGamePlayerGetter<T> where T : GamePlayer
    {   
        T GetPlayer(string playerId);
    }
}