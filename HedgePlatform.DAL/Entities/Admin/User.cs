using System;

namespace HedgePlatform.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Psw { get; set; }
    }
}
