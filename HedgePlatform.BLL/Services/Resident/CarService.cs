using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.BLL.Services
{
    public class CarService : ICarService
    {
        IUnitOfWork db { get; set; }

        public CarService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CarService>();

        public CarDTO GetCar(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var car = db.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("NOT_FOUND", "");

            Flat flat = db.Flats.Get(car.FlatId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Flat, FlatDTO>()).CreateMapper();

            return new CarDTO
            {
                Id = car.Id,
                FlatId = car.FlatId,
                flat = mapper.Map<Flat, FlatDTO>(flat),
                GosNumber = car.GosNumber
            };
        }

        public IEnumerable<CarDTO> GetCars()
        {

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Car, CarDTO>().ForMember(s => s.flat, h => h.MapFrom(src => src.Flat));
                cfg.CreateMap<Flat, FlatDTO>();
            }).CreateMapper();
            var cars = db.Cars.GetWithInclude(x => x.Flat);
            return mapper.Map<IEnumerable<Car>, List<CarDTO>>(cars);
        }

        public void CreateCar(CarDTO car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarDTO, Car>()).CreateMapper();
            try
            {
                db.Cars.Create(mapper.Map<CarDTO, Car>(car));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("car creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
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
                _logger.LogInformation("Edit car: " + car.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("car edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("car edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteCar(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var car = db.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Cars.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete car: " + car.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("car delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("car delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
