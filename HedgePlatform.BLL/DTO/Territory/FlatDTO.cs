using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.BLL.DTO
{
    public class FlatDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int HouseId { get; set; }
        public int MaxCounters { get; set; }
    }
}
