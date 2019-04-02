using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GameHub.Web.SignalR.Auth;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ConnectFourHub : Hub
    {
        //TODO: Manually purge completed or inactive games to prevent mem leak. maybe extract this functional into own service then inject that 
        static ConcurrentDictionary<string, IConnectFour> _games = new ConcurrentDictionary<string, IConnectFour>();

        static ConcurrentDictionary<string, string> _playerGameMap = new ConcurrentDictionary<string, string>();

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

            if (_playerGameMap[playerId] != gameId)
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

            if (createdSuccessfully)
            {
                Clients.Caller.SendAsync("RoomCreatedRedirect", Id);
            }
        }

        public void JoinRoom(string gameId, string playerNick, string playerColor)
        {
            // todo: check to make sure game exists first.
            var game = _games[gameId];

            var playerId = Context.Items["PlayerId"].ToString();

            bool isNewPlayer = playerId == null || !(_playerGameMap.ContainsKey(playerId) && _playerGameMap[playerId] == gameId);

            bool registeredSuccessfully;
            //not a returning player, so create id
            if (!isNewPlayer) return;
            {
                var player = new ConnectFourPlayer { Id = playerId, PlayerNick = playerNick, PlayerColor = playerColor };

                registeredSuccessfully = game.RegisterPlayer(player);
            }

            _playerGameMap.AddOrUpdate(playerId, gameId, (k,v) => gameId);
            
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
