using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Resident
    {
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
        [Required]
        public DateTime DateRegistration { get; set; }
        public DateTime DateChange { get; set; }
        [Required]
        public int? FlatId { get; set; }
        public Flat Flat { get; set; }
        [Required]
        public string ResidentStatus { get; set; }

    }
}
