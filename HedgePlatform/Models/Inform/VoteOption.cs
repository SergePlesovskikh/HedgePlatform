using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Inform
{
    public class VoteOption
    {
        public int Id { get; set; }

        [Display(Name = "Заголовок")]
        public int VoteId { get; set; }
        public virtual Vote Vote { get; set; }

        public string Description { get; set; }
    }
}
