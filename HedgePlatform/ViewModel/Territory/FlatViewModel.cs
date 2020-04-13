using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class FlatViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }

        [Required]
        public int HouseViewModelId { get; set; }
        public HouseViewModel House { get; set; }

        public int MaxCounters { get; set; }

        public virtual ICollection<CounterViewModel> Counters { get; set; }
        public virtual ICollection<ResidentViewModel> Resident { get; set; }
        public virtual ICollection<CarViewModel> Cars { get; set; }
    }
}
