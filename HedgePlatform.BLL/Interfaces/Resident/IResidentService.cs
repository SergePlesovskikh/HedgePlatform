using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IResidentService
    {
        ResidentDTO GetResident(int? id);
        string GetResidentStatus(int? id);
        void RegistrationResident(string uid, ResidentDTO resident);
        byte[] GetRequest(int? ResidentId);
        IEnumerable<ResidentDTO> GetResidents();
        ResidentDTO CreateResident(ResidentDTO resident);
        void EditResident(ResidentDTO resident);
        void DeleteResident(int? id);
        void Dispose();
    }
}
