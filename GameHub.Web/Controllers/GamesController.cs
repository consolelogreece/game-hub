using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace GameHub.Web.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {
        private IConfiguration _config;
        public GamesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("getgames")]
        public IActionResult GetGames()
        {
            return Ok("ok");
        }
    }
}