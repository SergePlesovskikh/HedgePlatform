using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Resident
    {
        public int Id { get; set; }
        // public virtual ICollection<Session> Sessions { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime DateRegistration { get; set; }
        public DateTime DateChange { get; set; }

        [Required]
        public int? FlatId { get; set; }
        public Flat Flat { get; set; }

    }
}
