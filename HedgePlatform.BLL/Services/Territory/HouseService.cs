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
    public class HouseService : IHouseService
    {
        private IUnitOfWork _db { get; set; }

        public HouseService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<HouseService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<House, HouseDTO>().ForMember(s => s.HouseManager, h => h.MapFrom(src => src.HouseManager));
            cfg.CreateMap<HouseManager, HouseManagerDTO>();
            cfg.CreateMap<HouseManagerDTO, HouseManager>();
        }).CreateMapper();

        public IEnumerable<HouseDTO> GetHouses()
        {
            var houses = _db.Houses.GetWithInclude(x => x.HouseManager);
            return _mapper.Map<IEnumerable<House>, List<HouseDTO>>(houses);
        }

        public void CreateHouse(HouseDTO house)
        {
            try
            {
                _db.Houses.Create(_mapper.Map<HouseDTO, House>(house));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"house creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"house creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void EditHouse(HouseDTO house)
        {
            if (house == null)
                throw new ValidationException("NO_OBJECT", "");
            try
            {
                _db.Houses.Update(_mapper.Map<HouseDTO, House>(house));
                _db.Save();
                _logger.LogInformation("Edit house: " + house.Id);
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"house edit error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteHouse(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "HOUSE_ID");

            var house = _db.Houses.Get(id.Value);
            if (house == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Houses.Delete(id.Value);
                _db.Save();
                _logger.LogInformation($"Delete house: {house.Id} - {house.Street} - {house.Home}");
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"house delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"house delete error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose() => _db.Dispose();        
    }
}
