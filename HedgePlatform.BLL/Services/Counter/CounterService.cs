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
            IUnitOfWork db { get; set; }
            private ICounterValueService _counterValueService;

            public CounterService(IUnitOfWork uow, ICounterValueService counterValueService)
            {
                db = uow;
                _counterValueService = counterValueService;
            }

            private readonly ILogger _logger = Log.CreateLogger<CounterService>();

            public CounterDTO GetCounter(int? id)
            {
                if (id == null)
                    throw new ValidationException("NULL", "");
                var counter = db.Counters.Get(id.Value);
                if (counter == null)
                    throw new ValidationException("NOT_FOUND", "");

                Flat flat = db.Flats.Get(counter.FlatId);
                CounterStatus counterStatus = db.CounterStats.Get(counter.CounterStatusId.Value);
                CounterType counterType = db.CounterTypes.Get(counter.CounterTypeId);

                var mapper = new MapperConfiguration(cfg => 
                { 
                    cfg.CreateMap<Flat, FlatDTO>(); 
                    cfg.CreateMap<CounterStatus,CounterStatusDTO>();
                    cfg.CreateMap<CounterType, CounterTypeDTO>();                
                }
                ).CreateMapper();

                return new CounterDTO
                {
                    Id = counter.Id,
                    FlatId = counter.FlatId,
                    Flat = mapper.Map<Flat, FlatDTO>(flat),
                    CounterStatusId = counter.CounterStatusId,
                    CounterStatus = mapper.Map<CounterStatus, CounterStatusDTO>(counterStatus),
                    CounterTypeId = counter.CounterTypeId,
                    CounterType = mapper.Map<CounterType, CounterTypeDTO>(counterType),
                    Location = counter.Location,
                    Number = counter.Number
                };
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

        public IEnumerable<CounterDTO> GetCountersByFlat(int FlatId)
        {
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
        }
}
