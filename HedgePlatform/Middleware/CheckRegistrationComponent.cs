using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Middleware
{
    public class CheckRegistrationComponent
    {
        private readonly RequestDelegate _next;
        private IPhoneService _phoneService;
        private IResidentService _residentService;

        public CheckRegistrationComponent(RequestDelegate next, IPhoneService phoneService, IResidentService residentService)
        {
            _next = next;
            _phoneService = phoneService;
            _residentService = residentService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                PhoneDTO phone = _phoneService.GetPhone((int)httpContext.Items["PhoneId"]);                
                if (phone.resident == null)
                {
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsync("NO_REGISTRATION");
                }
                else
                {                  
                    httpContext.Items["ResidentId"] = phone.resident.Id;
                    ResidentDTO resident = _residentService.GetResident(phone.resident.Id);
                    httpContext.Items["FlatId"] = resident.FlatId;                    
                    await _next(httpContext);
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
    public static class CheckRegistrationComponentExtensions
    {
        public static IApplicationBuilder UseCheckRegistrationComponent(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckRegistrationComponent>();
        }
    }
}
