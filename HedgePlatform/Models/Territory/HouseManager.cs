using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Territory
{
    public class HouseManager
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Дома")]
        public ICollection<House> Houses { get; set; }
    }
}
