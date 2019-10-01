using System;

namespace Caching.Common
{ 
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
}