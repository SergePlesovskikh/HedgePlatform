using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Worker
    {
        public int Id { get; set; }

        [Required]
        public string FIO { get; set; }
        public string Profession { get; set; }
        public string Phone { get; set; }
    }
}
