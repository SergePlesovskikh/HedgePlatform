
namespace HedgePlatform.BLL.DTO
{
    public class SessionDTO
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public int PhoneId { get; set; }
        public PhoneDTO Phone { get; set; }
    }
}
