using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Appeal
{
    public class Status
    {
        public int Id { get; set; }

        [Display(Name = "Статус")]
        public string Name { get; set; }
    }
}
