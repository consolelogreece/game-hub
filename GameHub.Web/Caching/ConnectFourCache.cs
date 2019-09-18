using System;
using System.Collections.Concurrent;
using System.Threading;
using FluentCache;
using GameHub.Games.BoardGames.ConnectFour;

// TODO: make sure expired games are purged.
namespace Caching
{
    public class ConnectFourCache
    {
        private readonly ConcurrentDictionary<string, Entry<ConnectFour>> _cache;

        private TimeSpan _expirationPeriod = TimeSpan.FromMinutes(15);

        private Timer _purgeTimer;

        public ConnectFourCache()
        {
            _cache = new ConcurrentDictionary<string, Entry<ConnectFour>>();

            _purgeTimer = new Timer((e) => this.Purge(), null, 0, (int)_expirationPeriod.TotalMilliseconds);
        }

        public void Set(string key, ConnectFour value)
        {
            var v = new Entry<ConnectFour>()
            {
                CacheDate = DateTimeOffset.Now,
                ExpirationDate = DateTimeOffset.Now + _expirationPeriod,
                LastAccessedDate = DateTimeOffset.Now,
                Value = (ConnectFour)value
            };
            
            _cache.TryAdd(key, v);
        }

        public ConnectFour Get(string key)
        {
            Entry<ConnectFour> entry;

            if(!_cache.TryGetValue(key, out entry))
                return null;
            

            var cachedValue = entry.ToCachedValue<ConnectFour>();

            return cachedValue.Value;
        }

        private void remove(string key)
        {
            _cache.TryRemove(key, out var entry);
        }

        private void Purge()
        {
            var now = DateTimeOffset.Now;

            foreach (var item in _cache)
            {
                if (now - this._expirationPeriod > item.Value.LastAccessedDate)
                {
                   this.remove(item.Key);
                }
            }
        }
    }


    internal class Entry<T>
    {
        public DateTimeOffset CacheDate { get; set; }
        public DateTimeOffset LastAccessedDate { get; set; }
        public object Value { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        public CachedValue<T> ToCachedValue<T>()
        {
            return new CachedValue<T>
            {
                CachedDate = CacheDate,
                Value = (T)Value
            };
        }
    }

    public class CachedValue<T>
    {
        public DateTimeOffset CachedDate { get; set; }

        public T Value { get; set; }
    }
}