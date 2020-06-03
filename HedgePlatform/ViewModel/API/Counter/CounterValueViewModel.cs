using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class CounterValueViewModel
    {
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        public DateTime DateValue { get; set; }
        [Required]
        public int CounterId { get; set; }
        public CounterViewModel Counter { get; set; }
        public byte[] Image { get; set; }
    }
}
