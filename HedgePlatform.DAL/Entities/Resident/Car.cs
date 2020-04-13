using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string GosNumber { get; set; }
        
        [Required]
        public int FlatId { get; set; }
        public Flat Flat { get; set; }

    }
}
