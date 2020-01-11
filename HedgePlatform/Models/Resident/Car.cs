using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Resident
{
    public class Car
    {
        public int Id { get; set; }

        [Display(Name = "Гос. номер")]
        public string GosNumber { get; set; }

        [Display(Name = "Квартира")]
        public int FlatId { get; set; }
        public Territory.Flat Flat { get; set; }

    }
}
