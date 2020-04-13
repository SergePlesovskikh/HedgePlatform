using System.Collections.Generic;


namespace HedgePlatform.DAL.Entities
{
    public class Vote : Message
    {
       public virtual ICollection<VoteOption> VoteOptions { get; set; }

    }
}
