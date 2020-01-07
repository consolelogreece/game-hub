using System.Collections.Generic;
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

            config.CreatorId = player.profile.Id;

            config.Rows = 10;
            config.Cols = 10;

            var ships = new List<ShipModel>();

            ships.Add(new ShipModel {
                Id = 0,
                col = 1,
                row = 1, 
                orientation = Orientation.Horizontal,
                length = 5
            });

            ships.Add(new ShipModel {
                Id = 1,
                col = 3,
                row = 7, 
                orientation = Orientation.Vertical,
                length = 4
            });

            ships.Add(new ShipModel {
                Id = 2,
                col = 1,
                row = 1, 
                orientation = Orientation.Horizontal,
                length = 4
            });

            ships.Add(new ShipModel {
                Id = 3,
                col = 5,
                row = 3, 
                orientation = Orientation.Vertical,
                length = 3
            });

            ships.Add(new ShipModel {
                Id = 4,
                col = 8,
                row = 4, 
                orientation = Orientation.Horizontal,
                length = 2
            });

            config.InitialShipLayout = ships;

            var game = new Battleships(config);

            _cache.Set(Id, game);

            return Ok(Id);
        }
    }
}