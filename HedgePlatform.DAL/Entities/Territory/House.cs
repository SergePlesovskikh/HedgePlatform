using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HedgePlatform.DAL.Entities
{
    public class House
    {
        public int Id { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Home { get; set; }
        public string Corpus { get; set; }
        public ICollection<Flat> Flats { get; set; }
        public int HouseManagerId { get; set; }
        public HouseManager HouseManager { get; set; }
    }
}
