using HedgePlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HedgePlatform.DAL
{
    public class HedgeDBContext : DbContext
    {
        //Migration Add-Migration NewMigration -Project HedgePlatform.DAL
        private readonly IConfiguration Configuration;
        public HedgeDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //Counter
        public DbSet<Counter> Counter { get; set; }
        public DbSet<CounterType> CounterType { get; set; }
        public DbSet<CounterValue> CounterValue { get; set; }
        public DbSet<CounterStatus> CounterStatus { get; set; }

        //Resident
        public DbSet<Car> Car { get; set; }
        public DbSet<Resident> Resident { get; set; }

        //Territory
        public DbSet<Flat> Flat { get; set; }
        public DbSet<House> House { get; set; }
        public DbSet<HouseManager> HouseManager { get; set; }

        //Inform
        public DbSet<Message> Message { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<VoteOption> VoteOption { get; set; }
        public DbSet<VoteResult> VoteResult { get; set; }

        //Admin
        public DbSet<User> User { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Check> Check { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgreConnection"), b => b.MigrationsAssembly("HedgePlatform.DAL"));

    }
}
