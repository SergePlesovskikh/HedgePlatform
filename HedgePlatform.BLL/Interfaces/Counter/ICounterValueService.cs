using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterValueService
    {
        CounterValueDTO GetCounterValue(int? id);
        IEnumerable<CounterValueDTO> GetCounterValues();
        void CreateCounterValue(CounterValueDTO counterValue);
        void EditCounterValue(CounterValueDTO counterValue);
        void DeleteCounterValue(int? id);
        void Dispose();
    }
}
