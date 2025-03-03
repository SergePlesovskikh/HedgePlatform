﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class HouseViewModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Corpus { get; set; }
        public ICollection<FlatViewModel> Flats { get; set; }
    }
}
