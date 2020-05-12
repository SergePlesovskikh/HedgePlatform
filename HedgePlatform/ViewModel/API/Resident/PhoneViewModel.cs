using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class PhoneViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        public int? ResidentId { get; set; }
        public ResidentViewModel Resident { get; set; }
    }
}
