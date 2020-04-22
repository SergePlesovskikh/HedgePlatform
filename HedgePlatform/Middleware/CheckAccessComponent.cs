using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using HedgePlatform.BLL.Interfaces;
using System;
using HedgePlatform.BLL.Infr;

namespace HedgePlatform.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckAccessComponent
    {
        private readonly RequestDelegate _next;
        private IResidentService _residentService;

        public CheckAccessComponent(RequestDelegate next, IResidentService residentService)
        {
            _next = next;
            _residentService = residentService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                string res_status = _residentService.GetResidentStatus((int)httpContext.Items["ResidentId"]);
                switch (res_status)
                {
                    case "Подтверждено":
                        await _next(httpContext);
                        break;
                    case "На рассмотрении":
                        httpContext.Response.StatusCode = 200;
                        await httpContext.Response.WriteAsync("IN_CHECK");
                        break;
                    case "Отклонено":
                        httpContext.Response.StatusCode = 200;
                        await httpContext.Response.WriteAsync("DECLAINED");
                        break;
                    default:
                        httpContext.Response.StatusCode = 500;
                        await httpContext.Response.WriteAsync("SERVER_ERROR");
                        break;
                }                   
            }
            catch (ValidationException ex)
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("SERVER_ERROR_" + ex.Message);
            }           
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckAccessComponentExtensions
    {
        public static IApplicationBuilder UseCheckAccessComponent(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckAccessComponent>();
        }
    }
}
