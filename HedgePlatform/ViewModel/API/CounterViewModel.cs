using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class CounterViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public int CounterTypeId { get; set; }
        public CounterTypeViewModel CounterType {get;set;}
        public CounterValueViewModel LastCounterValue { get; set; }
        public bool? AddValues { get; set; }
    }
}
