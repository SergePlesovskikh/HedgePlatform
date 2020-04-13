using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IHouseManagerService
    {
        HouseManagerDTO GetHouseManager(int? id);
        IEnumerable<HouseManagerDTO> GetHouseManagers();
        void CreateHouseManager(HouseManagerDTO houseManager);
        void EditHouseManager(HouseManagerDTO houseManager);
        void DeleteHouseManager(int? id);
        void Dispose();
    }
}
