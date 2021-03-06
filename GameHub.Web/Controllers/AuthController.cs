using Caching;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GameHub.Web.Controllers
{
    [Route("api/auth")]
    public class AuthContoller : Controller
    {
        private UserCache _users;

        public AuthContoller(UserCache users)
        {
            _users = users;
        }

        [HttpPost("signup")]
        public IActionResult Signup(string username)
        {
            //todo better validation
            if (username == null)
            {
                return BadRequest("Username not provided"); 
            }
            lock(_users)
            {
                var userMetaFromRequest = GetUserRequestMeta();

                if (userMetaFromRequest.isSignedIn)
                {
                    return BadRequest("User already signed in");
                }
                else if (_users.DoesUsernameExist(username))
                {
                    return BadRequest("Username in use");
                }
                else
                {
                    var id = userMetaFromRequest.profile.Id;

                    var newUser = new User {
                        Username = username,
                        Id = id
                    };

                    _users.Set(id, newUser);

                    return Ok(username);
                }
            }
        }

        [HttpGet("getusername")]
        public IActionResult GetUsername()
        {
            var user = GetUserRequestMeta();

            return Ok(user);
        }

        private UserRequestMeta GetUserRequestMeta()
        {
            return this.HttpContext.Items["user"] as UserRequestMeta;
        }
    }
}