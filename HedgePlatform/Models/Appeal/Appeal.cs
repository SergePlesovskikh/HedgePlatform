using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Appeal
{
    public class Appeal
    {
        public int Id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Обращение")]
        public string Message { get; set; }


        public int ResidentId { get; set; }

        public Resident.Resident Resident { get; set; }


        [Display(Name = "Статус")]
        public int StatusId { get; set; }

        public Status Status { get; set; }


        [Display(Name = "Работник")]
        public int? WorkerId { get; set; }

        public Worker Worker { get; set; }
    }
}
