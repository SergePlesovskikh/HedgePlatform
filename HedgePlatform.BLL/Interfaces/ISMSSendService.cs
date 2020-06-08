using System.Threading.Tasks;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ISMSSendService
    {
        public Task SendSMS(string phone);
        string token { get; }
    }
}
