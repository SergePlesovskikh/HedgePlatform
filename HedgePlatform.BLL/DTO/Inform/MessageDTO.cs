using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HedgePlatform.BLL.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DateMessage { get; set; }
    }
}
