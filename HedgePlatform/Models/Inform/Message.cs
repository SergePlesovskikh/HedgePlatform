using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.Models.Inform
{
    public class Message
    {
        public int Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Header { get; set; }

        [Display(Name = "Содержание")]
        public string Content { get; set; }

        [Display(Name = "Дата публикации")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateMessage { get; set; }
        

    }
}
