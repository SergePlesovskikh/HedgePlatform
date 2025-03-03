using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.BLL.Infr;
using HedgePlatform.DAL.Repositories;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Services;
using HedgePlatform.DAL;
using HedgePlatform.Middleware;
using DinkToPdf.Contracts;
using DinkToPdf;

namespace HedgePlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<HedgeDBContext>(options =>
            options.UseNpgsql("HedgeDBContext"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //DAL-services
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();

            //BLL-services
            services.AddTransient<ICheckService, CheckService>();
            services.AddTransient<IPhoneService, PhoneService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ICounterTypeService, CounterTypeService>();
            services.AddTransient<ICounterStatusService, CounterStatusService>();
            services.AddTransient<ICounterService, CounterService>();
            services.AddTransient<ICounterValueService, CounterValueService>();

            services.AddTransient<IHouseManagerService, HouseManagerService>();
            services.AddTransient<IHouseService, HouseService>();
            services.AddTransient<IFlatService, FlatService>();

            services.AddTransient<IResidentService, ResidentService>();
            services.AddTransient<ICarService, CarService>();

            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IVoteService, VoteService>();
            services.AddTransient<IVoteOptionService, VoteOptionService>();
            services.AddTransient<IVoteResultService, VoteResultService>();

            services.AddTransient<IHTMLService, HTMLService>();
            services.AddTransient<IPDFService, PDFService>();
            services.AddTransient<ISMSSendService, SMSSendService>();
            services.AddTransient<ITokenService, TokenService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.LoggerFactory = loggerFactory;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }          

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/mobile/regist"),
                appBuilder => appBuilder.UseMiddleware<CheckAuthComponent>());

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/mobile/work"),
             appBuilder2 => {
                 appBuilder2.UseMiddleware<CheckAuthComponent>();
                 appBuilder2.UseMiddleware<CheckRegistrationComponent>();
                 appBuilder2.UseMiddleware<CheckAccessComponent>();
             });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}
