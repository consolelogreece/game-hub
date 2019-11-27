using System;
using System.Threading.Tasks;
using Caching;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace GameHub.Web.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICache<User> userCache)
        {
            // currently ids are "protected" when sent to front end, so cant just paste in another players id as it hasn't been protected.
            var playerIdExists = context.Request.Cookies.ContainsKey("GHPID");

            var playerId = playerIdExists ? context.Request.Cookies["GHPID"] : null;

            var protector = DataProtectionProvider.Create("Gamehub.Web").CreateProtector("CookieEncryptPlayerId");

            var userRequestMeta = new UserRequestMeta();

            if (playerIdExists)
            {             
                playerId = protector.Unprotect(playerId);

                var userProfile = userCache.Get(playerId);

                userRequestMeta.isSignedIn = userProfile != null;
                userRequestMeta.profile = userProfile;

                var protectedPlayerId = protector.Protect(playerId);

                if (userRequestMeta.profile.Id == userProfile.Id)
                {
                    // todo: encode datetime expire with cookie to help date tampering.
                    // todo: perhaps remove ids and just use unique temporary names chosen on page load. check if in use like any other username.
                    // consider signed jwts or something
                    context.Response.Cookies.Append("GHPID", protectedPlayerId, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.Now + TimeSpan.FromHours(1),
                        SameSite = SameSiteMode.Strict
                    });
                }
                else
                // id provided in request is wrong, probably means user tampered with ID in a spoof attempt.
                {
                    context.Response.Cookies.Delete("GHPID");
                }    
            }

            context.Items.Add("user", userRequestMeta);
            
            await _next(context);
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}