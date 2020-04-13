using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.BLL.DTO
{
    public class CounterValueDTO
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime DateValue { get; set; }
        public int CounterId { get; set; }
        public byte[] Image { get; set; }
    }
}
