using Caching;
using GameHub.Games.BoardGames.ConnectFour;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
            var gameMeta = _config.GetSection("Games").Get<List<GameMeta>>();

            return Ok(gameMeta);
        }
    }
}