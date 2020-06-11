using System.Threading.Tasks;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Middleware
{   
    public class CheckDBComponent
    {
        private readonly RequestDelegate _next;
        private readonly ICheckDBConnectionService _checkDBConnectionService;

        public CheckDBComponent(RequestDelegate next, ICheckDBConnectionService checkDBConnectionService)
        {
            _next = next;
            _checkDBConnectionService = checkDBConnectionService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _checkDBConnectionService.CheckDBConnection();
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("SERVER_ERROR_" + ex.Message);
            }
        }
    }   
    public static class CheckDBComponentExtensions
    {
        public static IApplicationBuilder UseCheckDBComponent(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckDBComponent>();
        }
    }
}
