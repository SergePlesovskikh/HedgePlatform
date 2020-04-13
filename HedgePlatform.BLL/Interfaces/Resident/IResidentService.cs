using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IResidentService
    {
        ResidentDTO GetResident(int? id);
        IEnumerable<ResidentDTO> GetResident();
        void CreateResident(ResidentDTO resident);
        void EditResident(ResidentDTO resident);
        void DeleteResident(int? id);
        void Dispose();
    }
}
