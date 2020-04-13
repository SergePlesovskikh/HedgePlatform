using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Counter
    {

        public int Id { get; set; }

        [Required]
        public string Number { get; set; }        
        public string Location { get; set; }
      
        [Required]
        public int CounterTypeId { get; set; }
        public CounterType CounterType { get; set; }

        public int? CounterStatusId { get; set; }
        public CounterStatus CounterStatus { get; set; }

        [Required]
        public int FlatId { get; set; }
        public Flat Flat { get; set; }

        public ICollection<CounterValue> CounterValues { get; set; }

    }
}
