﻿using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICounterService
    {
        CounterDTO GetCounter(int? id);
        IEnumerable<CounterDTO> GetCounters();
        IEnumerable<CounterDTO> GetCountersByFlat(int? FlatId);
        bool CheckCounterToFlat(int FlatId, int CounterId);
        void CreateCounter(CounterDTO counter);
        void CreateCounter(CounterDTO counter, CounterValueDTO counterValue, int? FlatId);
        void EditCounter(CounterDTO counter);
        void DeleteCounter(int? id);
        void Dispose();
    }
}
