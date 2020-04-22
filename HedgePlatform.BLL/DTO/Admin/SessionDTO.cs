
namespace HedgePlatform.BLL.DTO
{
    public class SessionDTO
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public int? ResidentId { get; set; }

        public ResidentDTO Resident { get; set; }
    }
}
