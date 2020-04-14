using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.BLL.DTO
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string GosNumber { get; set; }
        public int FlatId { get; set; }
        public FlatDTO flat { get; set; }

    }
}
