using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class FlatViewModel
    {
        public int Id { get; set; }      
        public int Number { get; set; }
        public int HouseId { get; set; }
        public HouseViewModel House { get; set; }
    }
}
