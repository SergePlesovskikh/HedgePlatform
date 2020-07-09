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
        private IUnitOfWork _db { get; set; }

        public CarService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CarService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Car, CarDTO>().ForMember(s => s.flat, h => h.MapFrom(src => src.Flat));
            cfg.CreateMap<Flat, FlatDTO>();
            cfg.CreateMap<FlatDTO, Flat>();
        }).CreateMapper();
    
        public IEnumerable<CarDTO> GetCars()
        {
            var cars = _db.Cars.GetWithInclude(x => x.Flat);
            return _mapper.Map<IEnumerable<Car>, List<CarDTO>>(cars);
        }

        public void CreateCar(CarDTO car)
        {
            try
            {
                _db.Cars.Create(_mapper.Map<CarDTO, Car>(car));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"car creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"car creating error: {ex.Message}" );
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditCar(CarDTO car)
        {
            if (car == null)
                throw new ValidationException("NO_OBJECT", "");
            try
            {
                _db.Cars.Update(_mapper.Map<CarDTO, Car>(car));
                _db.Save();
                _logger.LogInformation($"Edit car: {car.Id}");
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Car edit Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Car edit error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteCar(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "CAR_ID");

            var car = _db.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Cars.Delete(id.Value);
                _db.Save();
                _logger.LogInformation($"Delete car: {car.Id} - {car.GosNumber}" );
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Car delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Car delete error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose() => _db.Dispose();
    }
}
