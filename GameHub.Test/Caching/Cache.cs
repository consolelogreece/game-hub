using Xunit;
using Caching;
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
            var cache = new Cache<string>(TimeSpan.FromSeconds(1));
Ca
        

        }
    }
}