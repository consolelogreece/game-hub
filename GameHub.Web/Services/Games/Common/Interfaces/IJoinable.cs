using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.Services.Games.Common
{
    public interface IJoinable
    {   
        ActionResult Join(string playerId, string playerNick);
    }
}