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
        private IUnitOfWork _db { get; set; }
        public CounterTypeService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterTypeService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => { 
            cfg.CreateMap<CounterType, CounterTypeDTO>();
            cfg.CreateMap<CounterTypeDTO, CounterType>();
        }).CreateMapper();

        public IEnumerable<CounterTypeDTO> GetCounterTypes()
        {            
            return _mapper.Map<IEnumerable<CounterType>, List<CounterTypeDTO>>(_db.CounterTypes.GetAll());
        }

        public void CreateCounterTypes(CounterTypeDTO counterType)
        {          
            try
            {
                _db.CounterTypes.Create(_mapper.Map<CounterTypeDTO, CounterType>(counterType));
                _db.Save();
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
            try
            {
                _db.CounterTypes.Update(_mapper.Map<CounterTypeDTO, CounterType>(counterType));
                _db.Save();
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
            var counterType = _db.CounterTypes.Get(id.Value);

            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");

            try
            {
                _db.CounterTypes.Delete(id.Value);
                _db.Save();
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
            _db.Dispose();
        }
    }
}
