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
        private IUnitOfWork _db { get; set; }  
        public FlatService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<FlatService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Flat, FlatDTO>().ForMember(s => s.House, h => h.MapFrom(src => src.House));
            cfg.CreateMap<House, HouseDTO>();
            cfg.CreateMap<FlatDTO, Flat>();
        }).CreateMapper();

        public FlatDTO GetFlat(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var flat = _db.Flats.Get(id.Value);
            if (flat == null)
                throw new ValidationException("NOT_FOUND", "");
            return _mapper.Map<Flat, FlatDTO>(flat);
        }      

        public IEnumerable<FlatDTO> GetFlats()
        {         
            var flats = _db.Flats.GetWithInclude(x => x.House);
            return _mapper.Map<IEnumerable<Flat>, List<FlatDTO>>(flats);
        }

        public IEnumerable<FlatDTO> GetFlats(int? HouseId)
        {            
            var flats = _db.Flats.Find(x => x.HouseId == HouseId);
            return _mapper.Map<IEnumerable<Flat>, List<FlatDTO>>(flats);
        }

        public void CreateFlat(FlatDTO flat)
        {            
            try
            {
                _db.Flats.Create(_mapper.Map<FlatDTO, Flat>(flat));
                _db.Save();
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
            try
            {
                _db.Flats.Update(_mapper.Map<FlatDTO, Flat>(flat));
                _db.Save();
                _logger.LogInformation("Edit flat: " + flat.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("flat edit _db error: " + ex.InnerException.Message);
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
            var flat = _db.Flats.Get(id.Value);
            if (flat == null)
                    throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Flats.Delete(id.Value);
                _db.Save();
                _logger.LogInformation("Delete flat: " + flat.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("flat delete _db error: " + ex.InnerException.Message);
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
            _db.Dispose();
        }
    }
}




