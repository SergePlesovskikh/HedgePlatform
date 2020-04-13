using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class CounterType
    {
        public int Id { get; set; }


        [Required]
        public string Type { get; set; }
    }
}
