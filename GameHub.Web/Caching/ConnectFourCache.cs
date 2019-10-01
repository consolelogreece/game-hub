using System;
using GameHub.Games.BoardGames.ConnectFour;

namespace Caching
{
    public class ConnectFourCache : CacheBase<ConnectFour>
    {
        public ConnectFourCache() : base(TimeSpan.FromMinutes(10))
        {       
        }
    }
}