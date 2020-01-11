using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Counter
{
    public class CounterType
    {
        public int Id { get; set; }

        [Display(Name = "Тип счетчика")]
        public string Type { get; set; }
    }
}
