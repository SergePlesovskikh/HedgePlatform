using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class HouseViewModel
    {
        public int Id { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Home { get; set; }
        public string Corpus { get; set; }
        public ICollection<FlatViewModel> Flats { get; set; }
        public int HouseManagerViewModelId { get; set; }
        public HouseManagerViewModel HouseManager { get; set; }
    }
}
