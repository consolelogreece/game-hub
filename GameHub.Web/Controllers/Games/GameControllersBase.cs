using Caching;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Web.Controllers.Games
{
    public class GameControllersBase<T> : Controller
    {
        protected ICache<T> _cache;

        protected UserRequestMeta player => Request.HttpContext.Items["user"] as UserRequestMeta;
        
        public GameControllersBase(ICache<T> cache)
        {
            _cache = cache;
        }
    }
}