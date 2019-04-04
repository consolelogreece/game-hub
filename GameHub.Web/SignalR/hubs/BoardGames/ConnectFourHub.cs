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
        static ConcurrentDictionary<string, IConnectFour> _games = new ConcurrentDictionary<string, IConnectFour>();

        static ConcurrentDictionary<string, ConcurrentBag<string>> _playerGameMap = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        public void StartGame(string gameId)
        {
            // TODO: Don't allow game to start without 2 players.
            var game = _games[gameId];

            var firstplayer = game.Start();

            Clients.Group(gameId).SendAsync("GameStarted", firstplayer.PlayerNick);
        }

        public void MakeMove(string gameId, int col)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            if (_playerGameMap[gameId].Contains(gameId) == false)
            {
                Clients.Caller.SendAsync("UnauthorizedAction");
                return;
            }

            var result = _games[gameId].MakeMove(col, playerId);

            if (result.WasValidMove)
            {
                if (result.DidMoveWin)
                {
                    Clients.Group(gameId).SendAsync("PlayerWon", result.PlayerNick);
                }
                else
                {
                    Clients.Group(gameId).SendAsync("PlayerMoved", result);
                }          
            }
            else
            {
                // TODO: Dedicated invalid move endpoint on front end
                Clients.Caller.SendAsync("InvalidMove", result);
            }
        }

        public void CreateRoom(ConnectFourConfiguration config)
        {
            var Id = Guid.NewGuid().ToString();

            var createdSuccessfully = _games.TryAdd(Id, new ConnectFour());

            if (createdSuccessfully && _playerGameMap.TryAdd(Id, new ConcurrentBag<string>()))
            {
                Clients.Caller.SendAsync("RoomCreatedRedirect", Id);
            }
            else
            {
                throw new HubException("Failed to create room");
            }
        }

        public void JoinRoom(string gameId, string playerNick, string playerColor)
        {
            // todo: check to make sure game exists first.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            bool isNewPlayer = _playerGameMap[gameId].Contains(playerId) == false;

            bool registeredSuccessfully;

            if (!isNewPlayer) return;
            {
                var player = new ConnectFourPlayer { Id = playerId, PlayerNick = playerNick, PlayerColor = playerColor };

                registeredSuccessfully = game.RegisterPlayer(player);
            }

            _playerGameMap[gameId].Add(playerId);
            
            if (Clients.Group(Context.ConnectionId) != null)
            {
                var boardState = GetGameStateColors(game);

                if (registeredSuccessfully)
                {
                    if (isNewPlayer)
                    {
                        Clients.Caller.SendAsync("RoomJoinedNewPlayer", playerId, boardState);
                    }
                    else
                    {
                        Clients.Caller.SendAsync("RoomJoinedReturningPlayer", boardState);
                    }     
                }
                else
                {
                    Clients.Caller.SendAsync("RoomJoinedSpectator", boardState);
                }

                Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            }
        }


        private string[][] GetGameStateColors(IConnectFour game)
        {
            var board = game.GetBoardState();

            var boardColorsOnly = new string[board.GetLength(0)][];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                var formattedRow = new string[board[i].Length];

                for (int j = 0; j < board[i].Length; j++)
                {
                    formattedRow[j] = board[i][j] == null ? "White" : board[i][j].PlayerColor;
                }

                boardColorsOnly[i] = formattedRow;
            }

            return boardColorsOnly;
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            if (!httpContext.Items.ContainsKey("GHPID")) Context.Abort();

            var ghpid = httpContext.Items["GHPID"].ToString();

            Context.Items.Add("PlayerId", ghpid);

            return base.OnConnectedAsync();
        }
    }
}
