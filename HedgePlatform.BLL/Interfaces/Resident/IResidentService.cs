using HedgePlatform.BLL.DTO;
using System.Collections.Generic;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IResidentService
    {
        ResidentDTO GetResident(int? id);
        string GetResidentStatus(int? id);
        bool CheckChairman(int ResidentId);
        void RegistrationResident(string uid, ResidentDTO resident);
        byte[] GetRequest(int? ResidentId);
        IEnumerable<ResidentDTO> GetResidents();
        ResidentDTO CreateResident(ResidentDTO resident);
        void EditResident(ResidentDTO resident);
        void DeleteResident(int? id);
        void Dispose();
    }
}
