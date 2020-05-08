using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class HouseManagerViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<HouseManagerViewModel> Houses { get; set; }
    }
}
