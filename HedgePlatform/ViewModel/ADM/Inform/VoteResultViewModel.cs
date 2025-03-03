﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.ViewModel
{
    public class VoteResultViewModel
    {
        public int Id { get; set; }

        [Required]
        public int VoteOptionId { get; set; }
        public VoteOptionViewModel VoteOption { get; set; }

        [Required]
        public int ResidentId { get; set; }
        public ResidentViewModel Resident { get; set; }

        [Required]
        public DateTime DateVote { get; set; }
    }
}
