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
    public class FlatService : IFlatService
    {
        IUnitOfWork db { get; set; }
        private readonly ILogger _logger = Log.CreateLogger<FlatService>();
        public FlatService(IUnitOfWork uow)
        {
            db = uow;
        }
        
        public FlatDTO GetFlat(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var flat = db.Flats.Get(id.Value);
            if (flat == null)
                throw new ValidationException("NOT_FOUND", "");

            return new FlatDTO
            {
                Id = flat.Id,
                HouseId = flat.HouseId,                
                MaxCounters = flat.MaxCounters,
                Number = flat.Number
            };
        }      

        public IEnumerable<FlatDTO> GetFlats()
        {
           var mapper = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Flat, FlatDTO>().ForMember(s => s.House, h => h.MapFrom(src => src.House));
                    cfg.CreateMap<House, HouseDTO>();
                }).CreateMapper();
                var flats = db.Flats.GetWithInclude(x => x.House);
                return mapper.Map<IEnumerable<Flat>, List<FlatDTO>>(flats);
        }

        public IEnumerable<FlatDTO> GetFlats(int? HouseId)
        {
            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Flat, FlatDTO>().ForMember(s => s.House, h => h.MapFrom(src => src.House));
                cfg.CreateMap<House, HouseDTO>();
            }).CreateMapper();
            var flats = db.Flats.Find(x => x.HouseId == HouseId);
            return mapper.Map<IEnumerable<Flat>, List<FlatDTO>>(flats);
        }

        public void CreateFlat(FlatDTO flat)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FlatDTO, Flat>()).CreateMapper();
            try
            {
                db.Flats.Create(mapper.Map<FlatDTO, Flat>(flat));
                db.Save();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            catch (Exception ex)
            {
                _logger.LogError("flat creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void EditFlat(FlatDTO flat)
        {
            if (flat == null)
                throw new ValidationException("No flat object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FlatDTO, Flat>()).CreateMapper();
            try
            {
                db.Flats.Update(mapper.Map<FlatDTO, Flat>(flat));
                db.Save();
                _logger.LogInformation("Edit flat: " + flat.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("flat edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            catch (Exception ex)
            {
                _logger.LogError("flat edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeleteFlat(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var flat = db.Flats.Get(id.Value);
            if (flat == null)
                    throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Flats.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete flat: " + flat.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("flat delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            catch (Exception ex)
            {
                _logger.LogError("flat delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}




