using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using HedgePlatform.BLL.Interfaces;

namespace HedgePlatform.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckAuthComponent
    {
        private readonly RequestDelegate _next;
        private ISessionService _sessionService;

        public CheckAuthComponent(RequestDelegate next, ISessionService sessionService)
        {
            _next = next;
            _sessionService = sessionService;
        }

        public async Task Invoke(HttpContext httpContext)
        {         
            var session = _sessionService.GetSession(httpContext.Request.Query["uid"].ToString());
            if (session == null)
            {
                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync("NONE");
            }
            else
            {
                httpContext.Items["PhoneId"] = session.PhoneId;
                await _next(httpContext);
            }           
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckAuthComponentExtensions
    {
        public static IApplicationBuilder UseCheckAuthComponent(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckAuthComponent>();
        }
    }
}
