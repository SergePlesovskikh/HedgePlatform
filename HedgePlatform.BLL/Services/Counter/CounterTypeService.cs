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
    public class CounterTypeService : ICounterTypeService
    {
        IUnitOfWork db { get; set; }

        public CounterTypeService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterTypeService>();

        public CounterTypeDTO GetCounterType(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var counterType = db.CounterTypes.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");

            return new CounterTypeDTO {Id = counterType.Id, Type = counterType.Type };
        }

        public IEnumerable<CounterTypeDTO> GetCounterTypes()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterType, CounterTypeDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<CounterType>, List<CounterTypeDTO>>(db.CounterTypes.GetAll());
        }

        public void CreateCounterTypes(CounterTypeDTO counterType)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterTypeDTO, CounterType>()).CreateMapper();
            try
            {
                db.CounterTypes.Create(mapper.Map<CounterTypeDTO, CounterType>(counterType));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Counter type creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");                
            }
         
        }
        public void EditCounterTypes(CounterTypeDTO counterType)
        {
            if (counterType == null)
                throw new ValidationException("No counter type object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterTypeDTO, CounterType>()).CreateMapper();
            try
            {
                db.CounterTypes.Update(mapper.Map<CounterTypeDTO, CounterType>(counterType));
                db.Save();
                _logger.LogInformation("Edit counter type: " + counterType.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Counter type edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Counter type edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }          
        }
        public void DeleteCounterType(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var counterType = db.CounterTypes.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.CounterTypes.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete counter type: " + counterType.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Counter type delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Counter type delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }            
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
