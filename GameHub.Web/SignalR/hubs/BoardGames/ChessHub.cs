using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Caching;
using GameHub.Games.BoardGames.Chess;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ChessDotNet;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ChessHub : Hub
    {
        // todo: register signalr connection on reconnect 

        private Chess game = new Chess();

        private ChessCache _cache; 
 
        public ChessHub([FromServices] ChessCache cache)
        {
            _cache = cache;
        }

        public void StartGame(string gameId)
        {
            // TODO: Don't allow game to start without 2 players.
        }

        public GameState GetGameState(string gameId)
        {
            return game.GetGameState();
        }

        public List<Move> GetMoves(string gameId)
        {
            return game.GetMoves(new ChessPlayer{player = ChessDotNet.Player.White});

            var playerId = Context.Items["PlayerId"].ToString();
            
            var player = game.GetPlayer(playerId);

            var moves = new List<Move>();

            if (player != null)
            {
                moves = game.GetMoves(player);
            }

            return moves;
        }

        public ChessPlayer GetClientPlayerInfo(string gameId)
        {    
            throw new NotImplementedException();     
        }

        public void MakeMove(Move move, string gameId)
        {
            throw new NotImplementedException();
        }

        public string CreateRoom()
        {
            throw new NotImplementedException();
        }

        public void JoinRoom(string gameId)
        {
            throw new NotImplementedException();
        }

        public void JoinGame(string gameId, string playerNick)
        {
            throw new NotImplementedException();
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
