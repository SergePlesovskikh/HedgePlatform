using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.ViewModel
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Uid { get; set; }

        public int? ResidentId { get; set; }

        public ResidentViewModel Resident { get; set; }
    }
}
