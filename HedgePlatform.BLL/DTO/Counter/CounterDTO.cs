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
        public int CounterTypeId { get; set; }
        public CounterTypeDTO CounterType { get; set; }
        public int? CounterStatusId { get; set; }
        public CounterStatusDTO CounterStatus { get; set; }
        public int FlatId { get; set; }
        public FlatDTO Flat { get; set; }
        public ICollection<CounterValueDTO> CounterValues { get; set; }
    }
}
