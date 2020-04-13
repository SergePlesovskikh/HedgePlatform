using System;

namespace HedgePlatform.DAL.Entities
{
    public class Session
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public int? ResidentId { get; set; }

        public Resident Resident { get; set; }
    }
}
