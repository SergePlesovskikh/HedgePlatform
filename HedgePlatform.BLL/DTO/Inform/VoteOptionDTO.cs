using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HedgePlatform.BLL.DTO
{
    public class VoteOptionDTO
    {
        public int Id { get; set; }

        [Required]
        public int VoteId { get; set; }
        public virtual VoteDTO Vote { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
