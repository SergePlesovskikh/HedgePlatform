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
    public class HouseManagerService : IHouseManagerService
    {
        IUnitOfWork db { get; set; }

        public HouseManagerService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<HouseManagerService>();

        public HouseManagerDTO GetHouseManager(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var houseManager = db.HouseManagers.Get(id.Value);
            if (houseManager == null)
                throw new ValidationException("NOT_FOUND", "");

            return new HouseManagerDTO { Id = houseManager.Id, Name = houseManager.Name, Image = houseManager.Image };
        }

        public IEnumerable<HouseManagerDTO> GetHouseManagers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManager, HouseManagerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<HouseManager>, List<HouseManagerDTO>>(db.HouseManagers.GetAll());
        }

        public void CreateHouseManager(HouseManagerDTO houseManager)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManagerDTO, HouseManager>()).CreateMapper();
            try
            {
                db.HouseManagers.Create(mapper.Map<HouseManagerDTO, HouseManager>(houseManager));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("House manager creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditHouseManager(HouseManagerDTO houseManager)
        {
            if (houseManager == null)
                throw new ValidationException("No House manager object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManagerDTO, HouseManager>()).CreateMapper();
            try
            {
                db.HouseManagers.Update(mapper.Map<HouseManagerDTO, HouseManager>(houseManager));
                db.Save();
                _logger.LogInformation("Edit House manager: " + houseManager.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("House manager edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("House manager edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteHouseManager(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var houseManager = db.HouseManagers.Get(id.Value);
            if (houseManager == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.HouseManagers.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete House manager: " + houseManager.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("House manager delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("House manager delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}