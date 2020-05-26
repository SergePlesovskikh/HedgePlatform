using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel.API
{
    public class ResidentViewModel
    {
        public int Id { get; set; }       
        public string FIO { get; set; }
        public DateTime BirthDate { get; set; }
        public int PhoneId { get; set; }
        public int? FlatId { get; set; }
    }
}
