using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.BLL.DTO
{
    public class ResidentDTO
    {
        public int Id { get; set; }      
        public string FIO { get; set; }
        public DateTime BirthDate { get; set; }
        public int PhoneId { get; set; }
        public PhoneDTO Phone { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateChange { get; set; }
        public int? FlatId { get; set; }
        public FlatDTO Flat { get; set; }
        public string Status { get; set; }
    }
}
