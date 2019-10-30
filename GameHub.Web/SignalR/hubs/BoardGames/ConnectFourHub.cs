using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using GameHub.Web.Services.Games.ConnectFourServices;
using System.Threading.Tasks;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        private IConnectFourServiceFactory _connectFourServiceFactory;
        public ConnectFourHub(IConnectFourServiceFactory connectFourServiceFactory)
        {
            _connectFourServiceFactory = connectFourServiceFactory;
        }

        private void ActionResultHandler(ActionResult result, string successEndpoint)
        {
            var gameId = Context.Items["GameId"].ToString();

            if (result.WasSuccessful)
            {
               Clients.Group(gameId).SendAsync(successEndpoint, GetGameService().GetGameState());
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", result.Message);
            }
        }

        public void StartGame(string playerId) 
        {
            var result = GetGameService().StartGame(playerId);

            ActionResultHandler(result, "GameStarted");
        }

        public void JoinGame(string playerId, string playerNick)
        {
            var plankerId = Context.Items["PlayerId"].ToString();
            var result = GetGameService().JoinGame(plankerId, playerNick);

            ActionResultHandler(result, "PlayerJoined");
        }

        public ConnectFourPlayer GetClientPlayerInfo(string uselessRemoveThisString)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            return GetGameService().GetPlayer(playerId);
        }

        public void Resign(string playerId) 
        {
            var result = GetGameService().Resign(playerId);

            ActionResultHandler(result, "PlayerResigned");
        }

        public void Rematch(string playerId) 
        {
            var result = GetGameService().Restart(playerId);

            ActionResultHandler(result, "RematchStarted");
        }

        public GameStateConnectFour GetGameState(string uselessRemoveThisString)
        {
           return GetGameService().GetGameState();
        }

        public void Move(string playerId, int col) 
        {
            var result = GetGameService().Move(playerId, col);

            ActionResultHandler(result, "PlayerMoved");
        }

        public virtual void JoinRoom(string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        private ConnectFourService GetGameService()
        {
            return Context.Items["GameService"] as ConnectFourService;
        }

        public override Task OnConnectedAsync()
        {
            // Get player id from http context. This is taken from a cookie and put in httpcontext items dictionary in an earlier piece of middleware.
            var httpContext = Context.GetHttpContext();

            if (httpContext.Items.ContainsKey("GHPID") == false)
            {
                throw new Exception("Got to hub without GHPID. This shouldn't happen, everybody panic!");
            }

            var gameId = httpContext.Request.Query["g"];

            var service = _connectFourServiceFactory.Create(gameId);

            if (service == null)
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Game doesn't exist"));
            }

            Context.Items.Add("GameService", service);

            var ghpid = httpContext.Items["GHPID"].ToString();

            // Store playerid in hub context.
            Context.Items.Add("PlayerId", ghpid);

            // Store gameid in hub context
            Context.Items.Add("GameId", gameId);

            return base.OnConnectedAsync();
        }
    }
}
