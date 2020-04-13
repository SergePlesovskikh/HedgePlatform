using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterService
    {
        CounterDTO GetCounter(int? id);
        IEnumerable<CounterDTO> GetCounters();
        void CreateCounter(CounterDTO counter);
        void EditCounter(CounterDTO counter);
        void DeleteCounter(int? id);
        void Dispose();
    }
}
