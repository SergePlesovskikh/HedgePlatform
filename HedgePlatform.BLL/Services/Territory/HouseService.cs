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

namespace HedgePlatform.BLL.Services.Territory
{
    public class HouseService : IHouseService
    {
        IUnitOfWork db { get; set; }

        public HouseService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<HouseService>();

        public HouseDTO GetHouse(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var house = db.Houses.Get(id.Value);
            if (house == null)
                throw new ValidationException("NOT_FOUND", "");

            HouseManager houseManager = db.HouseManagers.Get(house.HouseManagerId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManager, HouseManagerDTO>()).CreateMapper();

            return new HouseDTO { Id = house.Id, City = house.City, Corpus = house.Corpus, Home = house.Home,
                HouseManagerId = house.HouseManagerId, Street = house.Street, HouseManagerDTO = mapper.Map<HouseManager,HouseManagerDTO> (houseManager)
            };
        }

        public IEnumerable<HouseDTO> GetHouses()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<House, HouseDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<House>, List<HouseDTO>>(db.Houses.GetWithInclude(x=>x.HouseManager));
        }

        public void CreateHouse(HouseDTO house)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseDTO, House>()).CreateMapper();
            try
            {
                db.Houses.Create(mapper.Map<HouseDTO, House>(house));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("house creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditHouse(HouseDTO house)
        {
            if (house == null)
                throw new ValidationException("No house object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseDTO, House>()).CreateMapper();
            try
            {
                db.Houses.Update(mapper.Map<HouseDTO, House>(house));
                db.Save();
                _logger.LogInformation("Edit house: " + house.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("house edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("house edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteHouse(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var house = db.Houses.Get(id.Value);
            if (house == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Houses.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete house: " + house.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("house delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("house delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
