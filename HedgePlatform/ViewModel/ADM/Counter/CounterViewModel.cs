using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class CounterViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public int CounterTypeId { get; set; }
        public CounterTypeViewModel CounterType { get; set; }
        public int? CounterStatusId { get; set; }
        public CounterStatusViewModel CounterStatus { get; set; }
        public int FlatId { get; set; }
        public FlatViewModel Flat { get; set; }
        public ICollection<CounterValueViewModel> CounterValues { get; set; }
    }
}
