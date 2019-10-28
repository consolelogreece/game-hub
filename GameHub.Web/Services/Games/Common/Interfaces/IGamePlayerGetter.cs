using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.Services.Games.Common
{
    public interface IGamePlayerGetter
    {   
        GamePlayer GetPlayer(string playerId);
    }
}