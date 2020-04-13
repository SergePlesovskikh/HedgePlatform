using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class CounterValue
    {
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
       
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateValue { get; set; }

        [Required]
        public int CounterId { get; set; }

        public Counter Counter { get; set; }

        public byte[] Image { get; set; }
    }
}
