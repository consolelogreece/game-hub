using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConnectFourController : Controller
    {
        IConnectFour game;

        public ConnectFourController(IConnectFour c4)
        {
            game = c4;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("MakeMove")]
        public MoveResult MakeMove([FromQuery(Name = "column")] int col, [FromQuery(Name = "player")] string player)
        {
            return game.MakeMove(col, player);
        }   
    }
}
