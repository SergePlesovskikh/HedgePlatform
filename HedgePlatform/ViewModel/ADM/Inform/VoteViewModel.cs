﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class VoteViewModel : MessageViewModel
    {
        public virtual ICollection<VoteOptionViewModel> VoteOptions { get; set; }
    }
}
