using System;


namespace HedgePlatform.BLL.DTO
{
    public class CheckDTO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public int CheckCode { get; set; }
        public DateTime SendTime { get; set; }
        public string token { get; set; }
    }
}
