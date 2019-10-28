// public virtual void JoinGame(string gameId, string playerNick)
//         {
//             var game = _cache.Get(gameId);

//             if (game == null) 
//             {
//                 Clients.Caller.SendAsync("RoomDoesntExist");

//                 this.Context.Abort();

//                 return;
//             }

//             var playerId = Context.Items["PlayerId"].ToString();

//             var gamestate = game.GetGameState();  

//             var registerResult = new ActionResult(false); //default

//             // todo: dont do this check here, check in the game and return a value indicating success.
//             if (gamestate.Status.Status == GameHub.Games.BoardGames.Common.GameStatus.lobby.ToString())
//             {
//                 registerResult = game.RegisterPlayer(playerId, playerNick);
//             }

//             if (registerResult.WasSuccessful)
//             {
//                 Clients.Group(gameId).SendAsync("PlayerJoined", this.GetGameState(gameId));
//             }
//             else
//             {
//                 Clients.Caller.SendAsync("IllegalAction", registerResult.Message);
//             }
//         }

using Caching;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.ConnectFour;

public class GameJoiner
{
    
    private ConnectFour _game;

    look // change to IJoinable
    public GameJoiner(ConnectFour game)
    {
        _game = game;
    }
    
    public ActionResult JoinGame(string playerId, string playerNick)
    {
        lock(_game)
        {
            return _game.RegisterPlayer(playerId, playerNick);
        }
    }
}