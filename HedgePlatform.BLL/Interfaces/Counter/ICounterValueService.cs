﻿using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterValueService
    {
      
        bool CheckCounterToFlat(int FlatId, int CounterId);
        IEnumerable<CounterValueDTO> GetCounterValues();
        IEnumerable<CounterValueDTO> GetCounterValuesByCounter(int? CounterId);
        void CreateCounterValue(CounterValueDTO counterValue);
        void CreateCounterValue(CounterValueDTO counterValue, int? FlatId);
        void EditCounterValue(CounterValueDTO counterValue);
        void DeleteCounterValue(int? id);
        void Dispose();
    }
}
