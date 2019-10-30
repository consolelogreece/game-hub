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

        public void StartGame() 
        {
            var result = GetGameService().StartGame();

            ActionResultHandler(result, "GameStarted");
        }

        public void JoinGame(string playerNick)
        {
            var result = GetGameService().JoinGame(playerNick);

            ActionResultHandler(result, "PlayerJoined");
        }

        public ChessPlayer GetClientPlayerInfo()
        {
            return GetGameService().GetPlayer();
        }

        public void Resign() 
        {
            var result = GetGameService().Resign();

            ActionResultHandler(result, "PlayerResigned");
        }

        public void Rematch() 
        {
            var result = GetGameService().Restart();

            ActionResultHandler(result, "RematchStarted");
        }

        public GameStateChess GetGameState()
        {
           return GetGameService().GetGameState();
        }

        public void Move(Move move) 
        {
            var result = GetGameService().Move(move);

            ActionResultHandler(result, "PlayerMoved");
        }

        public List<Move> GetMoves() => GetGameService().GetMoves();

        private ChessService GetGameService()
        {
            return Context.Items["GameService"] as ChessService;
        }

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