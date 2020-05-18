using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class ResidentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public int PhoneId { get; set; }
        public PhoneViewModel Phone { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateChange { get; set; }
        public int? FlatId { get; set; }
        public FlatViewModel Flat { get; set; }
        [Required]
        public string ResidentStatus { get; set; }
        public bool? Chairman { get; set; }
        [Required]
        public bool Owner { get; set; }
    }
}
