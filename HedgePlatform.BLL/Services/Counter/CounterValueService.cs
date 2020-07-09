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
        private IUnitOfWork _db { get; set; }
        private IConfiguration _configuration;      

        public CounterValueService(IUnitOfWork uow, IConfiguration configuration)
        {
            _db = uow;
            _configuration = configuration;
        }

        private readonly ILogger _logger = Log.CreateLogger<CounterValueService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterValue, CounterValueDTO>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
            cfg.CreateMap<Counter, CounterDTO>();
            cfg.CreateMap<CounterValueDTO, CounterValue>();
        }).CreateMapper();

        public IEnumerable<CounterValueDTO> GetCounterValues()
        {
            var counterValues = _db.CounterValues.GetWithInclude(x => x.Counter);
            return _mapper.Map<IEnumerable<CounterValue>, List<CounterValueDTO>>(counterValues);
        }

        public IEnumerable<CounterValueDTO> GetCounterValuesByCounter(int? CounterId)
        {
            if (CounterId == null)
                throw new ValidationException("NULL", "COUNTER_ID");

            var counterValues = _db.CounterValues.GetWithInclude(p=>p.CounterId == CounterId,x => x.Counter);
            return _mapper.Map<IEnumerable<CounterValue>, List<CounterValueDTO>>(counterValues);
        }

        public void CreateCounterValue(CounterValueDTO counterValue)
        {           
            try
            {
                _db.CounterValues.Create(_mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"CounterValue creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"CounterValue creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void CreateCounterValue(CounterValueDTO counterValue, int? FlatId)
        {
            if (FlatId == null)
                throw new ValidationException("REQ_ERROR", "FLAT_ID");

            if (CheckCounterToFlat(FlatId.Value, counterValue.CounterId))
                throw new ValidationException("WRONG_COUNTER", "");

            if (!CheckCounterValueAdd(FlatId.Value))
                throw new ValidationException("NO_PERMISSION", "");
          
            try
            {
                _db.CounterValues.Create(_mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"CounterValue creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
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
            try
            {
                _db.CounterValues.Update(_mapper.Map<CounterValueDTO, CounterValue>(counterValue));
                _db.Save();
                _logger.LogInformation($"Edit counterValue: {counterValue.Id}");
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"counterValue edit Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"CounterValue edit error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteCounterValue(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "FLAT_ID");

            var counterValue = _db.CounterValues.Get(id.Value);
            if (counterValue == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.CounterValues.Delete(id.Value);
                _db.Save();
                _logger.LogInformation($"Delete counterValue: {counterValue.Id}");
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"СounterValue delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"СounterValue delete error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose() => _db.Dispose();
        

        public bool CheckCounterToFlat(int FlatId, int CounterId)
        {
            Counter counter = _db.Counters.Get(CounterId);
            return counter.FlatId == FlatId;
        }

        public bool CheckCounterValueAdd(int CounterId) => CheckAlwaysAdd() || (CheckDate() && CheckCurrentMonthVal(CounterId));        

        private bool CheckAlwaysAdd() => bool.Parse(_configuration["CounterOptions:always_send_counter_value"]);      

        private bool CheckDate()
        {
            int start_day = int.Parse(_configuration["CounterOptions:start_send_value_counter_day"]);
            int end_day = int.Parse(_configuration["CounterOptions:end_send_value_counter_day"]);
            return DateTime.Now.Day <= end_day && DateTime.Now.Day >= start_day;
        }

        private bool CheckCurrentMonthVal(int CounterId) => _db.CounterValues.FindFirst(x => ( x.CounterId == CounterId) 
            && (x.DateValue.Month==DateTime.Now.Month)) == null;      
    }
}
