using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HedgePlatform.Models.Territory
{
    public class House
    {
        public int Id { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Display(Name = "Дом")]
        public string Home { get; set; }

        [Display(Name = "Корпус")]
        public string Corpus { get; set; }

        [Display(Name = "Квартиры")]
        public ICollection<Flat> Flats { get; set; }

        [Display(Name = "Управляющая организация")]
        public int HouseManagerId { get; set; }
        public HouseManager HouseManager { get; set; }
    }
}
