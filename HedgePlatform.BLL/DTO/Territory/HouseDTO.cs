using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HedgePlatform.BLL.DTO
{
    public class HouseDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Corpus { get; set; }
        public int HouseManagerId { get; set; }
        public HouseManagerDTO HouseManagerDTO { get; set; }
    }
}
