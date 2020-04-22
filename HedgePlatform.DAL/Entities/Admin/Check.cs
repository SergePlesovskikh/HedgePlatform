﻿using System;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.DAL.Entities
{
    public class Check
    {
        public int Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int CheckCode { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        [Required]
        public string token { get; set; }
       
    }
}
