using System;
using GameHub.Web.Models;
using System.Linq;

namespace Caching
{
    public class UserCache : Cache<User>
    {
        public bool DoesUsernameExist(string username)
        {
            _cache.Any();

            return _cache.Any(e => ((User)e.Value.Value).Username == username);
        }

        public UserCache() : base(TimeSpan.FromHours(1))
        {       
        }
    }
}