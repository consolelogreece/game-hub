using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.AspNetCore.Mvc;
using Caching;
using GameHub.Games.BoardGames.Chess;
using System.Collections.Generic;
using ChessDotNet;

namespace GameHub.Web.SignalR.hubs.BoardGames
{
    public class ChessHub : BoardGameHubBase<Chess>
    {
        public ChessHub([FromServices] ICache<Chess> cache) : base(cache)
        {
        }

        public List<Move> GetMoves(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) 
            {
                Clients.Caller.SendAsync("RoomDoesntExist");

                this.Context.Abort();

                return null;
            }

            var playerId = Context.Items["PlayerId"].ToString();
            
            var player = game.GetPlayer(playerId);

            var moves = new List<Move>();

            if (player != null)
            {
                moves = game.GetMoves(player);
            }

            return moves;
        }

        public virtual void MakeMove(string gameId, Move move)
        {
            var playerId = Context.Items["PlayerId"].ToString();

            var game = _cache.Get(gameId);

            lock(game)
            {
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
    }
}