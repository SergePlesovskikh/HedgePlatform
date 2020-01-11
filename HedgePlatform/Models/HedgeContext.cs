using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.Models
{

    public class HedgeContext : DbContext
    {
        private readonly IConfiguration Configuration;
        public HedgeContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Admin.User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Configuration.GetValue<string>("db_connection_string"));
 

    }
}

