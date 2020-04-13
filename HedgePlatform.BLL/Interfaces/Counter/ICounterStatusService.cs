using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterStatusService
    {
        CounterStatusDTO GetCounterStatus(int? id);
        IEnumerable<CounterStatusDTO> GetCounterStats();
        void CreateCounterStatus(CounterStatusDTO counterStatus);
        void EditCounterStatus(CounterStatusDTO counterStatus);
        void DeleteCounterStatus(int? id);
        void Dispose();
    }
}
