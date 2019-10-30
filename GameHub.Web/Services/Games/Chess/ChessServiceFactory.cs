using Caching;
using GameHub.Games.BoardGames.Chess;
using GameHub.Games.BoardGames.ConnectFour;

namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public class ChessServiceFactory : IChessServiceFactory
    {
        private ICache<Chess> _cache;
        
        public ChessServiceFactory(ICache<Chess> cache)
        {
            _cache = cache;
        }

        public ChessService Create(string gameId, string playerId)
        {
            var game = _cache.Get(gameId);

            if (game == null) return null;

            return new ChessService(game, playerId);
        }
    }
}