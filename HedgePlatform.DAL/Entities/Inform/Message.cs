using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DateMessage { get; set; }
        public string Discriminator { get; set; }

    }
}
