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
    public class CounterStatusService : ICounterStatusService
    {
        private IUnitOfWork _db { get; set; }
        public CounterStatusService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterStatusService>();
        private static IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CounterStatus, CounterStatusDTO>();
            cfg.CreateMap<CounterStatusDTO, CounterStatus>();
        }).CreateMapper();

        public IEnumerable<CounterStatusDTO> GetCounterStats()
        {           
            return _mapper.Map<IEnumerable<CounterStatus>, List<CounterStatusDTO>>(_db.CounterStats.GetAll());
        }

        public void CreateCounterStatus(CounterStatusDTO counterStatus)
        {           
            try
            {
                _db.CounterStats.Create(_mapper.Map<CounterStatusDTO, CounterStatus>(counterStatus));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Counter status creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditCounterStatus(CounterStatusDTO counterStatus)
        {
            if (counterStatus == null)
                throw new ValidationException("No counter Status object", "");
          
            try
            {
                _db.CounterStats.Update(_mapper.Map<CounterStatusDTO, CounterStatus>(counterStatus));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Counter status edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Counter status edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeleteCounterStatus(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var counterType = _db.CounterStats.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.CounterStats.Delete(id.Value);
                _db.Save();
            }
            catch
            {
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
