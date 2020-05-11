﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class VoteResultViewModel
    {
        public int Id { get; set; }

        [Required]
        public int VoteOptionId { get; set; }
        public VoteOptionViewModel VoteOption { get; set; }

        [Required]
        public DateTime DateVote { get; set; }
    }
}
