using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class VoteOptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int VoteId { get; set; }
        public virtual VoteViewModel Vote { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
