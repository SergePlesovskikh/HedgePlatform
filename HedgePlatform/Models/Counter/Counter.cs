using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Counter
{
    public class Counter
    {

        public int Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Местонахождение")]
        public string Location { get; set; }

        [Display(Name = "Тип счетчика")]
        public string Type { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        public int FlatId { get; set; }
        public Territory.Flat Flat { get; set; }

        public ICollection<CounterValues> CounterValues { get; set; }

    }
}
