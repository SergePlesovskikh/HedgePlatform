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

namespace HedgePlatform.BLL.Services
{
    public class CounterService : ICounterService
    {
        private IUnitOfWork _db { get; set; }
        private ICounterValueService _counterValueService;
        private IFlatService _flatService;
        
        public CounterService(IUnitOfWork uow, ICounterValueService counterValueService, IFlatService flatService)
        {
            _db = uow;
            _counterValueService = counterValueService;
            _flatService = flatService;          
        }
        
        private readonly ILogger _logger = Log.CreateLogger<CounterService>();

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Counter, CounterDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
            .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
            .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
            cfg.CreateMap<Flat, FlatDTO>();
            cfg.CreateMap<CounterType, CounterTypeDTO>();
            cfg.CreateMap<CounterStatus, CounterStatusDTO>();
            cfg.CreateMap<CounterDTO, Counter>();
        }).CreateMapper();
        
        public IEnumerable<CounterDTO> GetCounters()
        {     
            var counters = _db.Counters.GetWithInclude(x => x.Flat, y=>y.CounterStatus,d =>d.CounterType);
            return _mapper.Map<IEnumerable<Counter>, List<CounterDTO>>(counters);
        }
        
        public IEnumerable<CounterDTO> GetCountersByFlat(int? FlatId)
        {
            if (FlatId == null)
                throw new ValidationException("FlatId is NULL", "");

            var counters = _db.Counters.GetWithInclude(p=>p.FlatId==FlatId, x => x.Flat, y => y.CounterStatus, d => d.CounterType);
            IEnumerable<CounterDTO> counterDTOs = _mapper.Map<IEnumerable<Counter>, List<CounterDTO>>(counters);
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
                _db.Counters.Create(mapper.Map<CounterDTO, Counter>(counter));
                _db.Save();
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
          
            try
            {
                counter.CounterStatusId = 1;
                counter.FlatId = FlatId.Value;
                Counter new_counter = _db.Counters.Create(_mapper.Map<CounterDTO, Counter>(counter));
                _db.Save();
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
            try
            {
                _db.Counters.Update(_mapper.Map<CounterDTO, Counter>(counter));
                _db.Save();
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

            var counter = _db.Counters.Get(id.Value);
            if (counter == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Counters.Delete(id.Value);
                _db.Save();
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
            _db.Dispose();
        }

        private bool CheckCounterAdd (int FlatId)
        {
            FlatDTO flat = _flatService.GetFlat(FlatId);
            return GetCountersByFlat(FlatId).Count()<flat.MaxCounters;                
        }
    }
}
