using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Flat
    {
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }

        [Required]
        public int HouseId { get; set; }
        public House House { get; set; }

        public int MaxCounters { get; set; }

        public virtual ICollection<Counter> Counters { get; set; }
        public virtual ICollection<Resident> Resident { get; set; }
        public virtual ICollection<Resident> Cars { get; set; }

    }
}
