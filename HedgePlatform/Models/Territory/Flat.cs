using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HedgePlatform.Models.Territory
{
    public class Flat
    {
        public int Id { get; set; }

        [Display(Name = "Номер квартиры")]
        public int Number { get; set; }

        [Display(Name = "Дом")]
        public int HouseId { get; set; }
        public House House { get; set; }

        [Display(Name = "Максимальное количество счетчиков")]
        public int MaxCounter { get; set; }

        [Display(Name = "Счетчики")]
        public virtual ICollection<Counter.Counter> Counters { get; set; }

        [Display(Name = "Жильцы")]
        public virtual ICollection<Resident.Resident> Resident { get; set; }

        [Display(Name = "Автомобили жильцов")]
        public virtual ICollection<Resident.Car> Cars { get; set; }

    }
}
