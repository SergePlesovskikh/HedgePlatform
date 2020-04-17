using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HedgePlatform.BLL.DTO
{
    public class VoteResultDTO
    {
        public int Id { get; set; }

        [Required]
        public int VoteOptionId { get; set; }
        public VoteOptionDTO VoteOption { get; set; }

        [Required]
        public int ResidentId { get; set; }
        public ResidentDTO Resident { get; set; }

        [Required]
        public DateTime DateVote { get; set; }
    }
}
