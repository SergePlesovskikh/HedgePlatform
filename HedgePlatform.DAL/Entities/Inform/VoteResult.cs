using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class VoteResult
    {
        public int Id { get; set; }

        [Required]
        public int VoteOptionId { get; set; }
        public VoteOption VoteOption { get; set; }

        [Required]
        public int ResidentId { get; set; }
        public Resident Resident { get; set; }

        [Required]
        public DateTime DateVote { get; set; }


    }
}
