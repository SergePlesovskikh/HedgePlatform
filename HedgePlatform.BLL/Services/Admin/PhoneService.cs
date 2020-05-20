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
    public class PhoneService : IPhoneService
    {
        IUnitOfWork db { get; set; }

        public PhoneService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<PhoneService>();
        public bool CheckPhone(string phone)
        {
            if (db.Phones.Find(x => x.Number == phone) != null)
                return true;
            else return false;
        }
        public PhoneDTO GetPhone(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var phone = db.Phones.Get(id.Value);
            if (phone == null)
                throw new ValidationException("NOT_FOUND", "");

            Resident resident = db.Residents.FindFirst(x => x.PhoneId == id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Resident, ResidentDTO>()).CreateMapper();

            return new PhoneDTO { Id = phone.Id, Number = phone.Number, resident = mapper.Map<Resident, ResidentDTO>(resident) };
        }

        public PhoneDTO GetOrCreate (string phone_number)
        {         
            if (phone_number == null)
                throw new ValidationException("NULL", "");

            var phone = db.Phones.FindFirst(x => x.Number == phone_number);
            if (phone != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Phone, PhoneDTO>()).CreateMapper();
                return mapper.Map<Phone, PhoneDTO>(phone);
            }               
            else
                return CreatePhone(new PhoneDTO { Number = phone_number });
        }

        public IEnumerable<PhoneDTO> GetPhones()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Phone, PhoneDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Phone>, List<PhoneDTO>>(db.Phones.GetAll());
        }

        public PhoneDTO CreatePhone(PhoneDTO phone)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhoneDTO, Phone>()).CreateMapper();
            try
            {
                Phone new_phone = db.Phones.Create(mapper.Map<PhoneDTO, Phone>(phone));
                db.Save();
                return mapper.Map<Phone, PhoneDTO>(new_phone);
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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhoneDTO, Phone>()).CreateMapper();
            try
            {
                db.Phones.Update(mapper.Map<PhoneDTO, Phone>(phone));
                db.Save();
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
            var counterType = db.Phones.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Phones.Delete(id.Value);
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
