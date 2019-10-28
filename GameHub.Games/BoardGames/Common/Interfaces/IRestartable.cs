using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Common
{
    public interface IRestartable
    {   
        // todo: shouldn't take playerid. figure out way to not call at all if the playerid is not that of the host.
        ActionResult Restart(string playerId);
    }
}