using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.Services.Games.Common
{
    public interface IGameStateGetter
    {   
        GameState GetState();
    }
}