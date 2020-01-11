using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Inform
{
    public class VoteResult
    {
        public int Id { get; set; }

        public int VoteOptionId { get; set; }
        public VoteOption VoteOption { get; set; }

        public int ResidentId { get; set; }
        public Resident.Resident Resident { get; set; }

        public DateTime DateVote { get; set; }


    }
}
