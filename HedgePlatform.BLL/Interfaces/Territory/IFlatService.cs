using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IFlatService
    {
        FlatDTO GetFlat(int? id);
        IEnumerable<FlatDTO> GetFlats();
        void CreateFlat(FlatDTO flat);
        void EditFlat(FlatDTO flat);
        void DeleteFlat(int? id);
        void Dispose();
    }
}
