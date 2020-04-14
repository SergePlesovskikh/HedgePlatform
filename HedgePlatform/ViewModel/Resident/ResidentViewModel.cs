﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.ViewModel
{
    public class ResidentViewModel
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateChange { get; set; }
        public int? FlatId { get; set; }
        public FlatViewModel Flat { get; set; }
    }
}
