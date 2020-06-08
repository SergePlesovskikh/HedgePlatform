using HedgePlatform.BLL.DTO;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IPhoneService
    {
        PhoneDTO GetPhone(int? id);
        bool CheckPhone(string phone);
        PhoneDTO GetOrCreate(string phone_number);
        PhoneDTO CreatePhone(PhoneDTO phone);
        void EditPhone(PhoneDTO phone);
        void DeletePhone(int? id);
        void Dispose();
    }
}
