using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Web.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Web.Controllers.Games
{
    [Route("api/connectfour")]
    public class ConnectFourController : GameControllersBase<ConnectFour>
    {
        public ConnectFourController(ICache<ConnectFour> cache) : base(cache)
        {}

        [AuthorizedActionFilter]
        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromBody]ConnectFourConfiguration config)
        {
            if (config == null)
            {
                return BadRequest();
            }

            var Id = System.Guid.NewGuid().ToString();

            config.CreatorId = player.profile.Id;

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