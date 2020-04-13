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
        IUnitOfWork db { get; set; }

        public CounterStatusService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterStatusService>();

        public CounterStatusDTO GetCounterStatus(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var counterStatus = db.CounterStats.Get(id.Value);
            if (counterStatus == null)
                throw new ValidationException("NOT_FOUND", "");

            return new CounterStatusDTO { Id = counterStatus.Id, Name = counterStatus.Name };
        }

        public IEnumerable<CounterStatusDTO> GetCounterStats()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatus, CounterStatusDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<CounterStatus>, List<CounterStatusDTO>>(db.CounterStats.GetAll());
        }

        public void CreateCounterStatus(CounterStatusDTO counterStatus)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatusDTO, CounterStatus>()).CreateMapper();
            try
            {
                db.CounterStats.Create(mapper.Map<CounterStatusDTO, CounterStatus>(counterStatus));
                db.Save();
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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatusDTO, CounterStatus>()).CreateMapper();
            try
            {
                db.CounterStats.Update(mapper.Map<CounterStatusDTO, CounterStatus>(counterStatus));
                db.Save();
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
            var counterType = db.CounterStats.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.CounterStats.Delete(id.Value);
                db.Save();
            }
            catch
            {
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
