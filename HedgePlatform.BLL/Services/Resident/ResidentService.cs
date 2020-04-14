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
    public class ResidentService : IResidentService
    {
        IUnitOfWork db { get; set; }

        public ResidentService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<ResidentService>();

        public ResidentDTO GetResident(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var resident = db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("NOT_FOUND", "");

            Flat flat = db.Flats.Get(resident.FlatId.Value);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Flat, FlatDTO>()).CreateMapper();

            return new ResidentDTO
            {
                Id = resident.Id,
                FlatId = resident.FlatId,
                Flat = mapper.Map<Flat, FlatDTO>(flat),
                FIO = resident.FIO,
                BirthDate = resident.BirthDate,
                DateChange = resident.DateChange,
                DateRegistration = resident.DateRegistration,
                Phone = resident.Phone
            };
        }

        public IEnumerable<ResidentDTO> GetResidents()
        {

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Resident, ResidentDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat));
                cfg.CreateMap<Flat, FlatDTO>();
            }).CreateMapper();
            var residents = db.Residents.GetWithInclude(x => x.Flat);
            return mapper.Map<IEnumerable<Resident>, List<ResidentDTO>>(residents);
        }

        public void CreateResident(ResidentDTO resident)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentDTO, Resident>()).CreateMapper();
            try
            {
                db.Residents.Create(mapper.Map<ResidentDTO, Resident>(resident));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("resident creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditResident(ResidentDTO resident)
        {
            if (resident == null)
                throw new ValidationException("No resident object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentDTO, Resident>()).CreateMapper();
            try
            {
                db.Residents.Update(mapper.Map<ResidentDTO, Resident>(resident));
                db.Save();
                _logger.LogInformation("Edit resident: " + resident.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("resident edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("resident edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteResident(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var resident = db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Residents.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete resident: " + resident.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("resident delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("resident delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}
