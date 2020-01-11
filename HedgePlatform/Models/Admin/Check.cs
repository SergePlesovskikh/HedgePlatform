using System;


namespace HedgePlatform.Models.Admin
{
    public class Check
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public string Phone { get; set; }

        public int CheckCode { get; set; }

        public DateTime SendTime { get; set; }

        public int? ResidentId { get; set; }

        public Resident.Resident Resident { get; set; }
    }
}
