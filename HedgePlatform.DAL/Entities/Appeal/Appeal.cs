using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Appeal
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateCreate { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int ResidentId { get; set; }
        public Resident Resident { get; set; }

        public int AppealStatusId { get; set; }
        public AppealStatus Status { get; set; }

        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
