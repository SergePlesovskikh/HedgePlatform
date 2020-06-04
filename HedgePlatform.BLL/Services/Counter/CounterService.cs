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
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HedgePlatform.BLL.Services
{
    public class CounterService : ICounterService
    {
        IUnitOfWork db { get; set; }
        private ICounterValueService _counterValueService;
        private IFlatService _flatService;
        
        public CounterService(IUnitOfWork uow, ICounterValueService counterValueService, IFlatService flatService)
        {
            db = uow;
            _counterValueService = counterValueService;
            _flatService = flatService;          
        }
        
        private readonly ILogger _logger = Log.CreateLogger<CounterService>();
        
        public CounterDTO GetCounter(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            //ToDo исправить метод, добавить в DAL
            var counter = db.Counters.GetWithInclude(x => x.Flat, y => y.CounterStatus, d => d.CounterType).First();
            if (counter == null)
                    throw new ValidationException("NOT_FOUND", "");

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Counter, CounterDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
                .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
                .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
                cfg.CreateMap<Flat, FlatDTO>();
                cfg.CreateMap<CounterType, CounterTypeDTO>();
                cfg.CreateMap<CounterStatus, CounterStatusDTO>();
            }).CreateMapper();

            return mapper.Map<Counter, CounterDTO>(counter); 
        }
        
        public IEnumerable<CounterDTO> GetCounters()
        {            
            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Counter, CounterDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
                .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
                .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
                cfg.CreateMap<Flat, FlatDTO>();
                cfg.CreateMap<CounterType, CounterTypeDTO>();
                cfg.CreateMap<CounterStatus, CounterStatusDTO>();
            }).CreateMapper();
            
            var counters = db.Counters.GetWithInclude(x => x.Flat, y=>y.CounterStatus,d =>d.CounterType);
            return mapper.Map<IEnumerable<Counter>, List<CounterDTO>>(counters);
        }
        
        public IEnumerable<CounterDTO> GetCountersByFlat(int? FlatId)
        {
            if (FlatId == null)
                throw new ValidationException("FlatId is NULL", "");

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Counter, CounterDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
                .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
                .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
                cfg.CreateMap<Flat, FlatDTO>();
                cfg.CreateMap<CounterType, CounterTypeDTO>();
                cfg.CreateMap<CounterStatus, CounterStatusDTO>();
            }).CreateMapper();

            var counters = db.Counters.GetWithInclude(p=>p.FlatId==FlatId, x => x.Flat, y => y.CounterStatus, d => d.CounterType);
            IEnumerable<CounterDTO> counterDTOs = mapper.Map<IEnumerable<Counter>, List<CounterDTO>>(counters);
            foreach (var counter in counterDTOs)
            {
                counter.CounterValues = _counterValueService.GetCounterValuesByCounter(counter.Id).ToList();
            }

            return counterDTOs;
        }

        public void CreateCounter(CounterDTO counter)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterDTO, Counter>()).CreateMapper();
            try
            {
                db.Counters.Create(mapper.Map<CounterDTO, Counter>(counter));
                db.Save();
            }
            
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            
            catch (Exception ex)
            {
                _logger.LogError("counter creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void CreateCounter(CounterDTO counter, CounterValueDTO counterValue, int? FlatId)
        {
            if (FlatId == null)
                throw new ValidationException("FlatId is NULL", "");

            if (!CheckCounterAdd(FlatId.Value))
                throw new ValidationException("Counters count is max", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterDTO, Counter>()).CreateMapper();
            try
            {
                counter.CounterStatusId = 1;
                counter.FlatId = FlatId.Value;
                Counter new_counter = db.Counters.Create(mapper.Map<CounterDTO, Counter>(counter));
                db.Save();
                counterValue.CounterId = new_counter.Id;
                _counterValueService.CreateCounterValue(counterValue);                
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("counter creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void EditCounter(CounterDTO counter)
        {
            if (counter == null)
                throw new ValidationException("No counter object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterDTO, Counter>()).CreateMapper();
            try
            {
                db.Counters.Update(mapper.Map<CounterDTO, Counter>(counter));
                db.Save();
                _logger.LogInformation("Edit counter: " + counter.Id);
            }
            
            catch (DbUpdateException ex)
            {
                _logger.LogError("counter edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            
            catch (Exception ex)
            {
                _logger.LogError("counter edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeleteCounter(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var counter = db.Counters.Get(id.Value);
            if (counter == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Counters.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete counter: " + counter.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("counter delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            
            catch (Exception ex)
            {
                _logger.LogError("counter delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        
        public void Dispose()
        {
            db.Dispose();
        }

        private bool CheckCounterAdd (int FlatId)
        {
            FlatDTO flat = _flatService.GetFlat(FlatId);
            return GetCountersByFlat(FlatId).Count()<flat.MaxCounters;                
        }
    }
}
