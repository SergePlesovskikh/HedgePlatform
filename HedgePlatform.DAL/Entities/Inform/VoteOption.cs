using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class VoteOption
    {
        public int Id { get; set; }

        [Required]
        public int VoteId { get; set; }
        public virtual Vote Vote { get; set; }

        [Required]
        public string Description { get; set; }
        public ICollection<VoteResult> VoteResults { get; set; }
    }
}
