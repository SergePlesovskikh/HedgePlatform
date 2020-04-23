using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Session
    {
        public int Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}
