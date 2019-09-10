using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        //TODO: Manually purge completed or inactive games to prevent mem leak. maybe extract this functional into own service then inject that 
        // if a game has had over 5 minutes without a move and no active connections, delete? or just delete after 15 mins without a move.
        // TODO: Find better way to transmit errors to user, throwing exceptions is expensive.

        // todo: if player joins after game starts it still sort of works. fix. to do this make sure to check if game has started BEFORE registering.
        // todo: register signalr connection on reconnect 
        static ConcurrentDictionary<string, IConnectFour> _games = new ConcurrentDictionary<string, IConnectFour>();

        public void StartGame(string gameId)
        {
            // TODO: Don't allow game to start without 2 players.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            var startedSuccessfully = game.Start(playerId);

            if (startedSuccessfully)
            {
                Clients.Group(gameId).SendAsync("GameStarted");
            }
            else
            {
                throw new HubException("You are not the game creator");
            }     
        }

        public void MakeMove(string gameId, int col)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _games[gameId];

            var result = game.MakeMove(col, playerId);

            if (result.WasValidMove)
            {
                Clients.Group(gameId).SendAsync("PlayerMoved", result);

                if (result.DidMoveWin)
                {
                    Clients.Group(gameId).SendAsync("PlayerWon", result.PlayerNick);
                }        
            }
            else
            {
                // TODO: Dedicated invalid move endpoint on front end
                Clients.Caller.SendAsync("InvalidMove", result);
            }
        }

        public string CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            if (!config.Validate()) throw new HubException("Invalid game config");

            var playerId = Context.Items["PlayerId"].ToString();

            config.creatorId = playerId;

            var createdSuccessfully = _games.TryAdd(Id, new ConnectFour(config));

            if (!createdSuccessfully)
            {
                throw new HubException("Failed to create room");
            }
            return Id;
        }

        public GameState JoinRoom(string gameId)
        {
            var playerId = Context.Items["PlayerId"].ToString();

               if (!_games.ContainsKey(gameId)) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return null;
            }

            Groups.AddToGroupAsync(Context.ConnectionId, gameId); 

            return _games[gameId].GetGameState(playerId);
        }

        public GameState JoinGame(string gameId, string playerNick)
        {
            if (!_games.ContainsKey(gameId)) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                return null;
            }

            // todo: stop breaking error on front end when player attempts to join but already has. perhaps make a check on componentdidmount and remove join option if already registered too.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            var gamestate = game.GetGameState(playerId);  

            var registeredSuccessfully = false;

            if (gamestate.Status == GameStatus.lobby.ToString())
            {
                registeredSuccessfully = game.RegisterPlayer(playerId, playerNick);
            }  

            return game.GetGameState(playerId);  
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
