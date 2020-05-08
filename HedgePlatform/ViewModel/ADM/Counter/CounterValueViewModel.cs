using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class CounterValueViewModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime DateValue { get; set; }
        public int CounterId { get; set; }
        public CounterViewModel Counter { get; set; }
        public byte[] Image { get; set; }
    }
}
