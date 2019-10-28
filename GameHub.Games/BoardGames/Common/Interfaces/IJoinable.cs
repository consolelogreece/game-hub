using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Common
{
    public interface IJoinable
    {   
        ActionResult Join(string playerId, string playerNick);
    }
}