using Caching;
using GameHub.Games.BoardGames.Chess;
using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;

namespace GameHub.Web.Controllers
{
    [Route("api/chess")]
    public class ChessController : Controller
    {
        ICache<Chess> _cache;
        public ChessController(ICache<Chess> cache)
        {
            _cache = cache;
        }
        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromBody]ChessConfiguration config)
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

            var game = new Chess(config);

            _cache.Set(Id, game);

            return Ok(Id);
        }
    }
}