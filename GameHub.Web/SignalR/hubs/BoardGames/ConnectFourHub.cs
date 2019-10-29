using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using GameHub.Web.Services.Games.ConnectFourServices;
using System.Threading.Tasks;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        private ConnectFourService _connectFourService;

        private IConnectFourServiceFactory _connectFourServiceFactory;
        public ConnectFourHub(IConnectFourServiceFactory connectFourServiceFactory)
        {
            _connectFourServiceFactory = connectFourServiceFactory;
        }
        
        public string CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            var playerId = Context.Items["PlayerId"].ToString();

            config.creatorId = playerId;

            var errors = config.Validate();

            if (errors.Count != 0)
            {
               Clients.Caller.SendAsync("IllegalConfiguration", errors);

               return null;
            }

            var game = new ConnectFour(config);

            _cache.Set(Id, game);

            return Id;
        }

        public override Task OnConnectedAsync()
        {
            // Get player id from http context. This is taken from a cookie and put in httpcontext items dictionary in an earlier piece of middleware.
            var httpContext = Context.GetHttpContext();

            if (httpContext.Items.ContainsKey("GHPID") == false)
            {
                throw new Exception("Got to hub without GHPID. This shouldn't happen, everybody panic!");
            }

            var ghpid = httpContext.Items["GHPID"].ToString();

            // Store playerid in hub context.
            Context.Items.Add("PlayerId", ghpid);

            return base.OnConnectedAsync();
        }
    }
}
