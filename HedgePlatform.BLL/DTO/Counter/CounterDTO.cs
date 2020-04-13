using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.BLL.DTO
{
    public class CounterDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public int? CounterStatusId { get; set; }
        public int FlatId { get; set; }

    }
}
