using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.DTO
{
    public class PhoneDTO
    {
        public int Id { get; set; }        
        public string Number { get; set; }
        public int? ResidentId { get; set; }
        public ResidentDTO resident { get; set; }
    }
}
