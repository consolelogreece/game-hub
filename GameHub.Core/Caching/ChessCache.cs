using System;
using GameHub.Games.BoardGames.Chess;

namespace Caching
{
    public class ChessCache : Cache<Chess>
    {
        public ChessCache() : base(TimeSpan.FromMinutes(30))
        {       
        }
    }
}