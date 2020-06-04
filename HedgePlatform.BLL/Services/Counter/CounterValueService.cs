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
using Microsoft.Extensions.Configuration;

namespace HedgePlatform.BLL.Services
{
    public class CounterValueService : ICounterValueService
    {
        IUnitOfWork db { get; set; }
        private IConfiguration _configuration;      

        public CounterValueService(IUnitOfWork uow, IConfiguration configuration)
        {
            db = uow;
            _configuration = configuration;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterValueService>();

        public CounterValueDTO GetCounterValue(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var counterValue = db.CounterValues.Get(id.Value);
            if (counterValue == null)
                throw new ValidationException("NOT_FOUND", "");

            Counter counter = db.Counters.Get(counterValue.CounterId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Counter, CounterDTO>()).CreateMapper();

            return new CounterValueDTO
            {
                Id = counterValue.Id,
                CounterId = counterValue.CounterId,
                Counter = mapper.Map<Counter, CounterDTO>(counter),
                DateValue = counterValue.DateValue,
                Image = counterValue.Image,
                Value = counterValue.Value
            };
        }

        public IEnumerable<CounterValueDTO> GetCounterValues()
        {

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CounterValue, CounterValueDTO>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
                cfg.CreateMap<Counter, CounterDTO>();
            }).CreateMapper();
            var counterValues = db.CounterValues.GetWithInclude(x => x.Counter);
            return mapper.Map<IEnumerable<CounterValue>, List<CounterValueDTO>>(counterValues);
        }

        public IEnumerable<CounterValueDTO> GetCounterValuesByCounter(int? CounterId)
        {
            if (CounterId == null)
                throw new ValidationException("NULL", "");

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CounterValue, CounterValueDTO>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
                cfg.CreateMap<Counter, CounterDTO>();
            }).CreateMapper();
            var counterValues = db.CounterValues.GetWithInclude(p=>p.CounterId == CounterId,x => x.Counter);
            return mapper.Map<IEnumerable<CounterValue>, List<CounterValueDTO>>(counterValues);
        }

        public void CreateCounterValue(CounterValueDTO counterValue)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterValueDTO, CounterValue>()).CreateMapper();
            try
            {
                db.CounterValues.Create(mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("counterValue creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void CreateCounterValue(CounterValueDTO counterValue, int? FlatId)
        {
            if (FlatId == null)
                throw new ValidationException("REQ_ERROR", "");

            if (CheckCounterToFlat(FlatId.Value, counterValue.CounterId))
                throw new ValidationException("WRONG_COUNTER", "");

            if (!CheckCounterValueAdd(FlatId.Value))
                throw new ValidationException("NO_PERMISSION", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterValueDTO, CounterValue>()).CreateMapper();
            try
            {
                db.CounterValues.Create(mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("counterValue creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditCounterValue(CounterValueDTO counterValue)
        {
            if (counterValue == null)
                throw new ValidationException("No counterValue object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterValueDTO, CounterValue>()).CreateMapper();
            try
            {
                db.CounterValues.Update(mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                db.Save();
                _logger.LogInformation("Edit counterValue: " + counterValue.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("counterValue edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("counterValue edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteCounterValue(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var counterValue = db.CounterValues.Get(id.Value);
            if (counterValue == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.CounterValues.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete counterValue: " + counterValue.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("counterValue delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("counterValue delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose()
        {
            db.Dispose();
        }

        public bool CheckCounterToFlat(int FlatId, int CounterId)
        {
            Counter counter = db.Counters.Get(CounterId);
            return counter.FlatId == FlatId;
        }

        public bool CheckCounterValueAdd(int CounterId)
        {
            return CheckAlwaysAdd() || (CheckDate() && CheckCurrentMonthVal(CounterId));
        }

        private bool CheckAlwaysAdd()
        {
            return bool.Parse(_configuration["CounterOptions:always_send_counter_value"]);
        }

        private bool CheckDate()
        {
            int start_day = int.Parse(_configuration["CounterOptions:start_send_value_counter_day"]);
            int end_day = int.Parse(_configuration["CounterOptions:end_send_value_counter_day"]);
            return DateTime.Now.Day <= end_day && DateTime.Now.Day >= start_day;
        }

        private bool CheckCurrentMonthVal(int CounterId)
        {
            return db.CounterValues.FindFirst(x => ( x.CounterId == CounterId) && (x.DateValue.Month==DateTime.Now.Month)) == null;
        }
    }
}
