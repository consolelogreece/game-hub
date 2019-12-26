using System;
using GameHub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameHub.Web.Filters.ActionFilters
{
    public class AuthorizedActionFilter : ActionFilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new System.NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var player = context.HttpContext.Request.HttpContext.Items["user"] as UserRequestMeta;

            if (player == null || !player.isSignedIn)
            {
                context.Result = new UnauthorizedObjectResult("Not signed in");
            }
            else if (player.profile.Id == null)
            {
                throw new Exception("Got to game controller without GHPID. This shouldn't happen, everybody panic!");
            }
        }
    }
}
