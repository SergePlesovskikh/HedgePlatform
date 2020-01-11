using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Appeal
{
    public class Worker
    {
        public int Id { get; set; }

        [Display(Name = "ФИО")]
        public string FIO { get; set; }

        [Display(Name = "Профессия")]
        public string Profession { get; set; }

        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }
    }
}
