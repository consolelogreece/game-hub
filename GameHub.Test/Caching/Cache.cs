using Xunit;
using Caching;
using System;
// sets properly
// gets properly
/// accessing updates expire time
// item deletes after

namespace GameHub.Test.Cache
{
    public class Getting
    {
        [Fact]
        public void GetsSetsProperly()
        {
            var cache = new Cache<string>();

            cache.Set("key", "value");

            var val = cache.Get("key");

            Assert.NotNull(val);

            Assert.Equal(val, "value");
        }
    }
}