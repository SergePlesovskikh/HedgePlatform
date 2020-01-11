using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedgePlatform.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Psw { get; set; }

        public string Sid { get; set; }

        public DateTime LastActivity { get; set; }

        public string LastAddr { get; set; }
    }
}
