using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System.Collections.Generic;

namespace HedgePlatform.BLL.Services
{
    public class CarService : ICarService
    {
        IUnitOfWork db { get; set; }

        public CarService(IUnitOfWork uow)
        {
            db = uow;
        }

        public CarDTO GetCar(int? id)
        {
            if (id == null)
                throw new ValidationException("Car id is null", "");
            var car = db.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("car is not found", "");

            return new CarDTO
            {
                Id = car.Id,
                FlatId = car.FlatId,
                GosNumber = car.GosNumber
            };
        }

        public IEnumerable<CarDTO> GetCars()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Car>, List<CarDTO>>(db.Cars.GetAll());
        }

        public void CreateCar(CarDTO car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarDTO, Car>()).CreateMapper();
            try
            {
                db.Cars.Create(mapper.Map<CarDTO, Car>(car));
                db.Save();
            }

            catch
            {
                throw new ValidationException("Error for creating car", "");
            }

        }
        public void EditCar(CarDTO car)
        {
            if (car == null)
                throw new ValidationException("No car object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarDTO, Car>()).CreateMapper();
            try
            {
                db.Cars.Update(mapper.Map<CarDTO, Car>(car));
                db.Save();
            }

            catch
            {
                throw new ValidationException("Error for editing Car", "");
            }
        }
        public void DeleteCar(int? id)
        {
            if (id == null)
                throw new ValidationException("Car id is null", "");
            var car = db.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("Car is not found", "");
            try
            {
                db.Cars.Delete(id.Value);
                db.Save();
            }
            catch
            {
                throw new ValidationException("Error for editing Car", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
