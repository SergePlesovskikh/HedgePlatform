using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.BLL.Services
{
    public class PhoneService : IPhoneService
    {
        private IUnitOfWork _db { get; set; }

        public PhoneService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<PhoneService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Phone, PhoneDTO>().ForMember(s => s.resident, h => h.MapFrom(src => src.Resident));
            cfg.CreateMap<Resident, ResidentDTO>();
            cfg.CreateMap<PhoneDTO, Phone>();
        }).CreateMapper();

        public bool CheckPhone(string phone)
        {
            return _db.Phones.Find(x => x.Number == phone) != null;             
        }

        public PhoneDTO GetPhone(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var phone = _db.Phones.GetOneWithInclude(x=>x.Id==id.Value, x=>x.Resident);
            if (phone == null)
                throw new ValidationException("NOT_FOUND", "");
            return _mapper.Map<Phone, PhoneDTO>(phone);
        }

        public PhoneDTO GetOrCreate (string phone_number)
        {         
            if (phone_number == null)
                throw new ValidationException("NULL", "");

            var phone = _db.Phones.FindFirst(x => x.Number == phone_number);
            if (phone != null)
            {
                return _mapper.Map<Phone, PhoneDTO>(phone);
            }               
            else
                return CreatePhone(new PhoneDTO { Number = phone_number });
        }
        public PhoneDTO CreatePhone(PhoneDTO phone)
        {
            try
            {
                Phone new_phone = _db.Phones.Create(_mapper.Map<PhoneDTO, Phone>(phone));
                _db.Save();
                _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Phone, PhoneDTO>()).CreateMapper();
                return _mapper.Map<Phone, PhoneDTO>(new_phone);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Phone creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditPhone(PhoneDTO phone)
        {
            if (phone == null)
                throw new ValidationException("No phone object", "");
            try
            {
                _db.Phones.Update(_mapper.Map<PhoneDTO, Phone>(phone));
                _db.Save();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Phone edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            catch (Exception ex)
            {
                _logger.LogError("Phone edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeletePhone(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var counterType = _db.Phones.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Phones.Delete(id.Value);
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
