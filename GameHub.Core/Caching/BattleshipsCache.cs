using System;
using GameHub.Games.BoardGames.Battleships;

namespace Caching
{
    public class BattleshipsCache : Cache<Battleships>
    {
        public BattleshipsCache() : base(TimeSpan.FromMinutes(10))
        {       
        }
    }
}