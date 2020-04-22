﻿using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        public string Psw { get; set; }
    }
}
