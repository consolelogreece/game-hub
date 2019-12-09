using Caching;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameHub.Web.Controllers
{
    [Route("api/auth")]
    public class AuthContoller : Controller
    {
        private ICache<User> _users;

        public AuthContoller(ICache<User> users)
        {
            _users = users;
        }

        [HttpPost("signup")]
        public IActionResult Signup(string username)
        {
            lock(_users)
            {
                var isUsernameInUse = _users.Get(username) != null;

                var userMetaFromRequest = GetUserRequestMeta();

                if (userMetaFromRequest.isSignedIn)
                {
                    return BadRequest("User already signed in");
                }
                else if (isUsernameInUse)
                {
                    return BadRequest("Username in use");
                }
                else
                {
                    var newId = Guid.NewGuid().ToString();

                    var newUser = new User {
                        Username = username,
                        Id = newId
                    };

                    _users.Set(newId, newUser);

                    Response.Cookies.Append("GHPID", newId);

                    return Ok(username);
                }
            }
        }

        [HttpPost("getusername")]
        public IActionResult GetUsername()
        {
            var user = GetUserRequestMeta();

            if (user.isSignedIn)
            {
                Ok(user.profile.Username);
            }

            return BadRequest("User not signed in");
        }

        private UserRequestMeta GetUserRequestMeta()
        {
            return this.HttpContext.Items["user"] as UserRequestMeta;
        }
    }
}