using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class AppealStatus
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
