using System;
using GameHub.Web.Models;

namespace Caching
{
    public class UserCache : Cache<User>
    {
        public UserCache() : base(TimeSpan.FromHours(1))
        {       
        }
    }
}