using Microsoft.AspNetCore.SignalR;
using System;
using GameHub.Web.Services.Games.ConnectFourServices;
using System.Threading.Tasks;
using GameHub.Games.BoardGames.Common;
using GameHub.Games.BoardGames.Chess;
using ChessDotNet;
using System.Collections.Generic;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ChessHub : Hub
    {
        private IChessServiceFactory _chessServiceFactory;
        public ChessHub(IChessServiceFactory chessServiceFactory)
        {
            _chessServiceFactory = chessServiceFactory;
        }

        public ChessGameState GetGameState()
        {
           return GetGameService().GetGameState();
        }

        private ChessService GetGameService()
        {
            return Context.Items["GameService"] as ChessService;
        }

        private void SendGamestate(string endpoint)
        {
            var gameState = GetGameState();

            var gameId = Context.Items["GameId"].ToString();

            Clients.Group(gameId).SendAsync(endpoint, gameState);
        }

        public ActionResult StartGame() 
        {
            var result = GetGameService().StartGame();

            if (result.WasSuccessful)
            {
                SendGamestate("GameStarted");
            }

            return result;
        }

        public ActionResult JoinGame(string playerNick)
        {
            var result = GetGameService().JoinGame(playerNick);

            if (result.WasSuccessful)
            {
                SendGamestate("PlayerJoined");
            }

            return result;
        }

        public ChessPlayer GetClientPlayerInfo()
        {
            return GetGameService().GetPlayer();
        }

        public ActionResult Resign() 
        {
            var result = GetGameService().Resign();

            if (result.WasSuccessful)
            {
                SendGamestate("PlayerResigned");
            }

            return result;
        }

        public ActionResult Rematch() 
        {
            var result = GetGameService().Restart();

            if (result.WasSuccessful)
            {
                SendGamestate("RematchStarted");
            }

            return result;
        }


        public ActionResult Move(Move move) 
        {
            var result = GetGameService().Move(move);

            if (result.WasSuccessful)
            {
                SendGamestate("PlayerMoved");
            }

            return result;
        }

        public List<Move> GetMoves() => GetGameService().GetMoves();


        public override Task OnConnectedAsync()
        {
            // Get player id from http context. This is taken from a cookie and put in httpcontext items dictionary in an earlier piece of middleware.
            var httpContext = Context.GetHttpContext();

            if (!httpContext.Items.ContainsKey("GHPID"))
            {
                throw new Exception("Got to hub without GHPID. This shouldn't happen, everybody panic!");
            }

            if (!httpContext.Request.Query.ContainsKey("g"))
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Game doesn't exist"));
            }

            var playerId = httpContext.Items["GHPID"].ToString();

            var gameId = httpContext.Request.Query["g"];

            var service = _chessServiceFactory.Create(gameId, playerId);

            if (service == null)
            {
                this.Context.Abort();

                return base.OnDisconnectedAsync(new Exception("Game doesn't exist"));
            }

            Context.Items.Add("GameService", service);

            // Store playerid in hub context.
            Context.Items.Add("PlayerId", playerId);

            // Store gameid in hub context
            Context.Items.Add("GameId", gameId);

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            return base.OnConnectedAsync();
        }
    }
}