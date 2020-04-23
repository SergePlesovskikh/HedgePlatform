using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IPhoneService
    {
        PhoneDTO GetPhone(int? id);
        bool CheckPhone(string phone);
        PhoneDTO GetOrCreate(string phone_number);
        IEnumerable<PhoneDTO> GetPhones();
        PhoneDTO CreatePhone(PhoneDTO phone);
        void EditPhone(PhoneDTO phone);
        void DeletePhone(int? id);
        void Dispose();
    }
}
