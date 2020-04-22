using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HedgePlatform.DAL.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        public int? ResidentId { get; set; }
        public Resident resident { get; set; }
    }
}
