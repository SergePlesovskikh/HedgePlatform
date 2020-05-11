using System;

namespace HedgePlatform.BLL.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime DateMessage { get; set; }
    }
}
