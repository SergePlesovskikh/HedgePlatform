using HedgePlatform.DAL.Entities;
using HedgePlatform.DAL.Interfaces;
using System;
using Microsoft.Extensions.Configuration;


namespace HedgePlatform.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork 
    {
        private HedgeDBContext db;

        private AbstractRepository<Counter> counterRepository;
        private AbstractRepository<CounterStatus> сounterStatsRepository;
        private AbstractRepository<CounterValue> сounterValuesRepository;
        private AbstractRepository<CounterType> сounterTypesRepository;

        private AbstractRepository<Car> carRepository;
        private AbstractRepository<Resident> residentRepository;

        private AbstractRepository<Flat> flatRepository;
        private AbstractRepository<House> houseRepository;
        private AbstractRepository<HouseManager> houseManagerRepository;

        private readonly IConfiguration Configuration;

        public EFUnitOfWork(IConfiguration configuration)
        {
            Configuration = configuration;          
            db = new HedgeDBContext(Configuration);
        }

        public IRepository<Counter> Counters { get { return counterRepository ?? new AbstractRepository<Counter>(db); }}
        public IRepository<CounterValue> CounterValues { get { return сounterValuesRepository ?? new AbstractRepository<CounterValue>(db); } }
        public IRepository<CounterStatus> CounterStats { get { return сounterStatsRepository ?? new AbstractRepository<CounterStatus>(db); } }
        public IRepository<CounterType> CounterTypes { get { return сounterTypesRepository ?? new AbstractRepository<CounterType>(db); } }

        public IRepository<Car> Cars { get { return carRepository ?? new AbstractRepository<Car>(db); } }
        public IRepository<Resident> Residents { get { return residentRepository ?? new AbstractRepository<Resident>(db); } }

        public IRepository<Flat> Flats { get { return flatRepository ?? new AbstractRepository<Flat>(db); } }
        public IRepository<House> Houses { get { return houseRepository ?? new AbstractRepository<House>(db); } }
        public IRepository<HouseManager> HouseManagers { get { return houseManagerRepository ?? new AbstractRepository<HouseManager>(db); } }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
