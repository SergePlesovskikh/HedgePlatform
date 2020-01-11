using System;

namespace HedgePlatform.Models.Admin
{
    public class Session
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public int? ResidentId { get; set; }

        public Resident.Resident Resident { get; set; }
    }
}
