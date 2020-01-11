using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Counter
{
    public class CounterValues
    {
        public int Id { get; set; }

        [Display(Name = "Показания")]
        public double Value { get; set; }

        [Display(Name = "Дата показаний")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateValue { get; set; }

        public int CounterId { get; set; }

        public Counter Counter { get; set; }

        public byte[] Image { get; set; }
    }
}
