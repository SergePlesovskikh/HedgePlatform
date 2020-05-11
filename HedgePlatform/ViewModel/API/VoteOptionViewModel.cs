using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class VoteOptionViewModel
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public  VoteViewModel Vote { get; set; }
        public string Description { get; set; }
    }
}
