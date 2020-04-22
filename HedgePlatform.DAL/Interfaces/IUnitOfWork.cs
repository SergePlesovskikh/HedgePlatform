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

        IRepository<Message> Messages { get; }
        IRepository<Vote> Votes { get; }
        IRepository<VoteOption> VoteOptions { get; }
        IRepository<VoteResult> VoteResults { get; }

        IRepository<User> Users { get; }
        IRepository<Session> Sessions { get; }
        IRepository<Check> Checks { get; }
        IRepository<Phone> Phones { get; }

        void Save();
    }
}
