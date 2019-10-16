using System;

namespace Caching.Common
{
    public class CachedValue<T>
    {
        public DateTimeOffset CachedDate { get; set; }

        public T Value { get; set; }
    }
}