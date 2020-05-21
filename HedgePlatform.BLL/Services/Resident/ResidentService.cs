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
    public class ResidentService : IResidentService
    {
        IUnitOfWork db { get; set; }
        private ISessionService _sessionService;
        private IPhoneService _phoneService;
        private IHouseService _houseService;
        private IFlatService _flatService;
        private IHTMLService _HTMLService;
        private IPDFService _PDFService;

        public ResidentService(IUnitOfWork uow, ISessionService sessionService, IPhoneService phoneService, IFlatService flatService, 
            IHouseService houseService, IHTMLService HTMLService, IPDFService PDFService)
        {
            db = uow;
            _sessionService = sessionService;
            _phoneService = phoneService;
            
            _HTMLService = HTMLService;
            _PDFService = PDFService;
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
            Phone phone = db.Phones.Get(resident.PhoneId);

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Flat, FlatDTO>();
                cfg.CreateMap<Phone, PhoneDTO>();
            }).CreateMapper();

            return new ResidentDTO
            {
                Id = resident.Id,
                FlatId = resident.FlatId,
                Flat = mapper.Map<Flat, FlatDTO>(flat),
                FIO = resident.FIO,
                BirthDate = resident.BirthDate,
                DateChange = resident.DateChange,
                DateRegistration = resident.DateRegistration,
                PhoneId = resident.PhoneId,
                Phone = mapper.Map<Phone, PhoneDTO>(phone),
                ResidentStatus = resident.ResidentStatus
            };
        }

        public string GetResidentStatus (int? id)
        {
            return GetResident(id).ResidentStatus;
        }

       public byte[] GetRequest(int? ResidentId)
        {
            if (ResidentId == null)
                throw new ValidationException("NULL", "");
            var residents = db.Residents.GetWithInclude(x => x.Phone, x => x.Flat, x=>x.Flat.House);
            if (residents == null)
                throw new ValidationException("NOT_FOUND", "");

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Resident, ResidentDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
                 .ForMember(s => s.Phone, h => h.MapFrom(src => src.Phone))
                 .ForMember(s => s.Flat.House, h => h.MapFrom(src => src.Flat.House));
                cfg.CreateMap<Flat, FlatDTO>();
                cfg.CreateMap<Phone, PhoneDTO>();
                cfg.CreateMap<House, House>();
            }).CreateMapper();

            string html = _HTMLService.GenerateHTMLRequest(mapper.Map<Resident, ResidentDTO>(residents.FirstOrDefault()));
            return _PDFService.PdfConvert(html);
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

        public void RegistrationResident(string uid, ResidentDTO resident)
        {
            SessionDTO session = _sessionService.GetSession(uid);   
            PhoneDTO phone = _phoneService.GetPhone(session.PhoneId);

            CheckNullResidentData(phone, resident, uid);
           
            HouseDTO house = _houseService.GetHouse(resident.Flat.HouseId);
            if (house == null)
            {
               throw new ValidationException("HOUSE_NOT_FOUND", "");
            }

            FlatDTO flat = _flatService.GetFlat(resident.FlatId);
            if (flat == null)
            {
               throw new ValidationException("FLAT_NOT_FOUND", "");
            }

            resident = ResidentBuilder(resident, phone, flat);
            ResidentDTO new_resident = CreateResident(resident);
            phone.resident = new_resident;

        }

        private void CheckNullResidentData (PhoneDTO phone, ResidentDTO resident, string uid)
        {

            if (phone == null)
            {
                _logger.LogError("Not found phone for uid. Uid=" + uid);
                throw new ValidationException("SERVER_ERROR", "");
            }

            if (resident.Flat == null)
            {
                _logger.LogError("Not found flat object" + uid);
                throw new ValidationException("REQUEST_ERROR", "");
            }

            if (resident.Flat.House == null)
            {
                _logger.LogError("Not found house object" + uid);
                throw new ValidationException("REQUEST_ERROR", "");
            }
        }

        private ResidentDTO ResidentBuilder (ResidentDTO resident, PhoneDTO phone, FlatDTO flat)
        {
            resident.PhoneId = phone.Id;
            resident.Phone = phone;
            resident.FlatId = flat.Id;
            resident.Flat = flat;
            resident.Flat.HouseId = flat.HouseId;
            return resident;
        }
       

        public ResidentDTO CreateResident(ResidentDTO resident)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentDTO, Resident>()).CreateMapper();
            try
            {
                Resident new_resident =  db.Residents.Create(mapper.Map<ResidentDTO, Resident>(resident));
                db.Save();
                return mapper.Map<Resident, ResidentDTO>(new_resident);
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
