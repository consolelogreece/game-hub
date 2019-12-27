using Caching;
using GameHub.Games.BoardGames.Chess;
using GameHub.Web.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Web.Controllers.Games
{
    [Route("api/chess")]
    public class ChessController : GameControllersBase<Chess>
    {
        public ChessController(ICache<Chess> cache) : base(cache)
        {}

        [AuthorizedActionFilter]
        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromBody]ChessConfiguration config)
        {
            if (config == null)
            {
                return BadRequest();
            }

            var Id = System.Guid.NewGuid().ToString();

            config.creatorId = player.profile.Id;

            var game = new Chess(config);

            _cache.Set(Id, game);

            return Ok(Id);
        }
    }
}