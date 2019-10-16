using System;
using GameHub.Games.BoardGames.ConnectFour;

namespace Caching
{
    public class ConnectFourCache : Cache<ConnectFour>
    {
        public ConnectFourCache() : base(TimeSpan.FromMinutes(10))
        {       
        }
    }
}