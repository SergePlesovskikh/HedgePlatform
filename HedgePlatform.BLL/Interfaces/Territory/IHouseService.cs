using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IHouseService
    {      
        IEnumerable<HouseDTO> GetHouses();
        void CreateHouse(HouseDTO house);
        void EditHouse(HouseDTO house);
        void DeleteHouse(int? id);
        void Dispose();
    }
}
