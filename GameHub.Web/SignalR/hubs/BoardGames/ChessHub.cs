using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Caching;
using GameHub.Games.BoardGames.Chess;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ChessDotNet;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ChessHub : Hub
    {
        // todo: register signalr connection on reconnect 
        // todo: make sure to check whether game exists where neccessary such as in makemove.

        private ChessCache _cache; 
 
        public ChessHub([FromServices] ChessCache cache)
        {
            _cache = cache;
        }

        public void StartGame(string gameId)
        {
            var game = _cache.Get(gameId);

            var started = game.StartGame();

            if (started)
            {
                Clients.Group(gameId).SendAsync("GameStarted", game.GetGameState());
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", "Couldn't start game");
            }
        }

        public GameState GetGameState(string gameId)
        {
            return _cache.Get(gameId).GetGameState();
        }

        public List<Move> GetMoves(string gameId)
        {
            var game = _cache.Get(gameId);

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
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            var player = game.GetPlayer(playerId);

            return player;
        }

        public void MakeMove(Move move, string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            var isValid = game.MakeMove(playerId, move);

            if (isValid)
            {
                Clients.Group(gameId).SendAsync("PlayerMoved", GetGameState(gameId));
            }
            else
            {
                Clients.Caller.SendAsync("IllegalAction", "Invalid move");
            }
        }

        public string CreateRoom()
        {
            var Id = Guid.NewGuid().ToString();

            var playerId = Context.Items["PlayerId"].ToString();

            var game = new Chess(new ChessConfig
            {
                creatorId = playerId
            });

            _cache.Set(Id, game);

            return Id;
        }

        public void JoinRoom(string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return;
            }

            Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public bool JoinGame(string gameId, string playerNick)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return false;
            }

            var playerId = Context.Items["PlayerId"].ToString();

            var gamestate = game.GetGameState();  

            var registeredSuccessfully = false;

            if (gamestate.Status.Status == GameHub.Games.BoardGames.Common.GameStatus.lobby.ToString())
            {
                registeredSuccessfully = game.RegisterPlayer(playerId, playerNick);
            }

            return registeredSuccessfully;
        }

        public void Rematch(string gameId)
        {
            var game = _cache.Get(gameId);

            var playerId = Context.Items["PlayerId"].ToString();

            if (game == null) return;

            var resetSuccessful = game.Reset(playerId);

            if (resetSuccessful)
            {
                Clients.Group(gameId).SendAsync("GameStarted", this.GetGameState(gameId));
            }
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