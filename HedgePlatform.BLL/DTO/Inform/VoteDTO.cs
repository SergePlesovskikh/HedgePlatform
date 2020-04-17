using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.DTO
{
    public class VoteDTO : MessageDTO
    {
        public virtual ICollection<VoteOptionDTO> VoteOptions { get; set; }
    }
}
