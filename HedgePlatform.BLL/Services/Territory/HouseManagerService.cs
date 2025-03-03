﻿using AutoMapper;
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
        private IUnitOfWork _db { get; set; }
        public HouseManagerService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<HouseManagerService>();
        private static IMapper _mapper = new MapperConfiguration(cfg =>{
            cfg.CreateMap<HouseManager, HouseManagerDTO>();
            cfg.CreateMap<HouseManagerDTO, HouseManager>();
        }).CreateMapper();
      
        public IEnumerable<HouseManagerDTO> GetHouseManagers() => _mapper.Map<IEnumerable<HouseManager>, List<HouseManagerDTO>>(_db.HouseManagers.GetAll());        

        public void CreateHouseManager(HouseManagerDTO houseManager)
        {
            try
            {
                _db.HouseManagers.Create(_mapper.Map<HouseManagerDTO, HouseManager>(houseManager));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"House manager creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"House manager creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void EditHouseManager(HouseManagerDTO houseManager)
        {
            if (houseManager == null)
                throw new ValidationException("NO_OBJECT", "");
            try
            {
                _db.HouseManagers.Update(_mapper.Map<HouseManagerDTO, HouseManager>(houseManager));
                _db.Save();
                _logger.LogInformation($"Edit House manager: {houseManager.Id}");
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"House manager edit Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"House manager edit error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeleteHouseManager(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "HOUSE_MANAGER_ID");

            var houseManager = _db.HouseManagers.Get(id.Value);
            if (houseManager == null)
                throw new ValidationException("NOT_FOUND", "");

            try
            {
                _db.HouseManagers.Delete(id.Value);
                _db.Save();
                _logger.LogInformation($"Delete House manager: {houseManager.Id} - {houseManager.Name}");
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"House manager delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"House manager delete error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose() => _db.Dispose();        
    }
}