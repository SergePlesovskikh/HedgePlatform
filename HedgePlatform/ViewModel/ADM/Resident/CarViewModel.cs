using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string GosNumber { get; set; }
        public int FlatId { get; set; }
        public FlatViewModel Flat { get; set; }
    }
}
