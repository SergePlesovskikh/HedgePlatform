using HedgePlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        IRepository<Counter> Counters { get; }
        IRepository<CounterStatus> CounterStats { get; }
        IRepository<CounterValue> CounterValues { get; }
        IRepository<CounterType> CounterTypes { get; }

        IRepository<Car> Cars { get; }
        IRepository<Resident> Residents { get; }

        IRepository<Flat> Flats { get; }
        IRepository<House> Houses { get; }
        IRepository<HouseManager> HouseManagers { get; }

        void Save();
    }
}
