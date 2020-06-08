using System;
using System.Collections.Generic;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterTypeService
    {
        IEnumerable<CounterTypeDTO> GetCounterTypes();
        void CreateCounterTypes(CounterTypeDTO counterType);
        void EditCounterTypes(CounterTypeDTO counterType);
        void DeleteCounterType(int? id);
        void Dispose();
    }
}
