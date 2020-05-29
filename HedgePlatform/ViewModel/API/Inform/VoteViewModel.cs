using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class VoteViewModel : MessageViewModel
    {
        public ICollection<VoteOptionViewModel> VoteOptions { get; set; }
    }
}
