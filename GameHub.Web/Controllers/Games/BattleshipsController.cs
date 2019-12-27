using Caching;
using GameHub.Games.BoardGames.Battleships;
using GameHub.Web.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Web.Controllers.Games
{
    [Route("api/battleships")]
    public class BattleshipsController : GameControllersBase<Battleships>
    {
        public BattleshipsController(ICache<Battleships> cache) : base(cache)
        {}

        [AuthorizedActionFilter]
        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromBody]BattleshipsConfiguration config)
        {
            if (config == null)
            {
                return BadRequest();
            }

            var Id = System.Guid.NewGuid().ToString();

            config.creatorId = player.profile.Id;

            var game = new Battleships(config);

            _cache.Set(Id, game);

            return Ok(Id);
        }
    }
}