using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Common
{
    public interface IMoveable<T>
    {   
        ActionResult Move(string playerId, T move);
    }
}