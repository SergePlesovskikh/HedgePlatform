using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICarService
    {
        CarDTO GetCar(int? id);
        IEnumerable<CarDTO> GetCars();
        void CreateCar(CarDTO flat);
        void EditCar(CarDTO flat);
        void DeleteCar(int? id);
        void Dispose();
    }
}
