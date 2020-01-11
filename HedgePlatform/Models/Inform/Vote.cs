using System.Collections.Generic;


namespace HedgePlatform.Models.Inform
{
    public class Vote : Message
    {
       public virtual ICollection<VoteOption> VoteOptions { get; set; }

    }
}
