using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;

namespace GameHub.Web.Controllers
{
    public class ConnectFourController : Controller
    {
        ICache<ConnectFour> _cache;
        public ConnectFourController(ICache<ConnectFour> cache)
        {
            _cache = cache;
        }
        [HttpPost]
        public IActionResult CreateRoom(ConnectFourConfiguration config)
        {
            var Id = System.Guid.NewGuid().ToString();

            var playerId = Request.Cookies["GHPID"];

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