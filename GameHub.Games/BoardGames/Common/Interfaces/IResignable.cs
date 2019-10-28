using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Common
{
    public interface IResignable
    {   
        ActionResult Resign(string playerId);
    }
}