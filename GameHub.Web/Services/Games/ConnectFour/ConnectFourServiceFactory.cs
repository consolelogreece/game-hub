using Caching;
using GameHub.Games.BoardGames.ConnectFour;

namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public class ConnectFourServiceFactory : IConnectFourServiceFactory
    {
        private ICache<GameHub.Games.BoardGames.ConnectFour.ConnectFour> _cache;
        
        public ConnectFourServiceFactory(ICache<ConnectFour> cache)
        {
            _cache = cache;
        }

        public ConnectFourService Create(string gameId)
        {
            var game = _cache.Get(gameId);

            if (game == null) return null;

            return new ConnectFourService(game);
        }
    }
}