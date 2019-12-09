using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameHub.Web.Controllers
{
    [Route("api/connectfour")]
    public class ConnectFourController : Controller
    {
        ICache<ConnectFour> _cache;
        public ConnectFourController(ICache<ConnectFour> cache)
        {
            _cache = cache;
        }
        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromBody]ConnectFourConfiguration config)
        {
            if (config == null)
            {
                return BadRequest();
            }

            var Id = System.Guid.NewGuid().ToString();

            var playerId = Request.HttpContext.Items["GHPID"].ToString();

            if (playerId == null)
            {
                throw new Exception("Got to game controller without GHPID. This shouldn't happen, everybody panic!");
            }

            config.creatorId = playerId;

            var errors = config.Validate();

            if (errors.Count != 0)
            {
               return BadRequest(errors);
            }

            var game = new ConnectFour(config);

            _cache.Set(Id, game);

            return Ok(Id);
        }
    }
}