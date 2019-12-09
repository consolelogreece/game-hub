using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Caching.Common;

namespace Caching
{
    public class Cache<T> : ICache<T> where T : class
    {
        internal readonly ConcurrentDictionary<string, Entry<T>> _cache;

        internal TimeSpan _expirationPeriod;

        internal Timer _purgeTimer;

        public Cache() : this(TimeSpan.FromMinutes(15))
        {}

        public Cache(TimeSpan expirationPeriod)
        {
            _expirationPeriod = expirationPeriod;

            _cache = new ConcurrentDictionary<string, Entry<T>>();

            _purgeTimer = new Timer((e) => this.Purge(), null, 0, (int)_expirationPeriod.TotalMilliseconds);
        }

        public void Set(string key, T value)
        {
            var v = new Entry<T>()
            {
                CacheDate = DateTimeOffset.Now,
                ExpirationDate = DateTimeOffset.Now + _expirationPeriod,
                LastAccessedDate = DateTimeOffset.Now,
                Value = (T)value
            };
            
            _cache.TryAdd(key, v);
        }

        public T Get(string key)
        {
            Entry<T> entry;

            if(!_cache.TryGetValue(key, out entry))
                return null;
            

            var cachedValue = entry.ToCachedValue();

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
}