using Caching;
using GameHub.Games.BoardGames.Battleships;

namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public class BattleshipsServiceFactory : IBattleshipsServiceFactory
    {
        private ICache<Battleships> _cache;
        
        public BattleshipsServiceFactory(ICache<Battleships> cache)
        {
            _cache = cache;
        }

        public BattleshipsService Create(string gameId, string playerId)
        {
            var game = _cache.Get(gameId);

            if (game == null) return null;

            return new  BattleshipsService(game, playerId);
        }
    }
}