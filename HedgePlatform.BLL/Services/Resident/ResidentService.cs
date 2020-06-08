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
        private IUnitOfWork _db { get; set; }
        private ISessionService _sessionService;
        private IPhoneService _phoneService;
        private IFlatService _flatService;
        private IHTMLService _HTMLService;
        private IPDFService _PDFService;

        public ResidentService(IUnitOfWork uow, ISessionService sessionService, IPhoneService phoneService, IFlatService flatService, 
            IHTMLService HTMLService, IPDFService PDFService)
        {
            _db = uow;
            _sessionService = sessionService;
            _phoneService = phoneService;          
            _flatService = flatService;
            _HTMLService = HTMLService;
            _PDFService = PDFService;
        }

        private readonly ILogger _logger = Log.CreateLogger<ResidentService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Resident, ResidentDTO>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
             .ForMember(s => s.Phone, h => h.MapFrom(src => src.Phone));
            cfg.CreateMap<Flat, FlatDTO>();
            cfg.CreateMap<Phone, PhoneDTO>();
            cfg.CreateMap<House, HouseDTO>();
        }).CreateMapper();

        public ResidentDTO GetResident(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var resident = _db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("NOT_FOUND", "");
            return _mapper.Map<Resident, ResidentDTO>(resident);
        }

        public string GetResidentStatus (int? id)
        {
            return GetResident(id).ResidentStatus;
        }

        public byte[] GetRequest(int? ResidentId)
        {
            if (ResidentId == null)
                throw new ValidationException("NULL", "");
            var residents = _db.Residents.GetWithInclude(x => x.Phone, x => x.Flat, x=>x.Flat.House);
            if (residents == null)
                throw new ValidationException("NOT_FOUND", "");

            string html = _HTMLService.GenerateHTMLRequest(_mapper.Map<Resident, ResidentDTO>(residents.FirstOrDefault()));
            return _PDFService.PdfConvert(html);
        }

        public IEnumerable<ResidentDTO> GetResidents()
        {
            var residents = _db.Residents.GetWithInclude(x => x.Flat);
            return _mapper.Map<IEnumerable<Resident>, List<ResidentDTO>>(residents);
        }

        public bool CheckChairman(int ResidentId)
        {
            return GetResident(ResidentId).Chairman.Value;
        }

        public void RegistrationResident(string uid, ResidentDTO resident)
        {
            SessionDTO session = _sessionService.GetSession(uid);   
            PhoneDTO phone = _phoneService.GetPhone(session.PhoneId);

            CheckNullResidentData(phone, resident, uid);

            if (_flatService.GetFlat(resident.FlatId) == null)
            {
                throw new ValidationException("FLAT_NOT_FOUND", "");
            }             

            resident = ResidentBuilder(resident, phone);
            ResidentDTO new_resident = CreateResident(resident);
            phone.resident = new_resident;
        }

        public ResidentDTO CreateResident(ResidentDTO resident)
        {          
            try
            {
                Resident new_resident =  _db.Residents.Create(_mapper.Map<ResidentDTO, Resident>(resident));
                _db.Save();
                _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Resident, ResidentDTO>()).CreateMapper();
                return _mapper.Map<Resident, ResidentDTO>(new_resident);
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
          
            try
            {
                _db.Residents.Update(_mapper.Map<ResidentDTO, Resident>(resident));
                _db.Save();
                _logger.LogInformation("Edit resident: " + resident.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("resident edit _db error: " + ex.InnerException.Message);
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

            var resident = _db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Residents.Delete(id.Value);
                _db.Save();
                _logger.LogInformation("Delete resident: " + resident.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("resident delete _db error: " + ex.InnerException.Message);
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
            _db.Dispose();
        }

        private void CheckNullResidentData(PhoneDTO phone, ResidentDTO resident, string uid)
        {
            if (phone == null)
            {
                _logger.LogError("Not found phone for uid. Uid=" + uid);
                throw new ValidationException("SERVER_ERROR", "");
            }

            if (resident.FlatId == null)
            {
                _logger.LogError("Not found flat object" + uid);
                throw new ValidationException("REQUEST_ERROR", "");
            }
        }

        private ResidentDTO ResidentBuilder(ResidentDTO resident, PhoneDTO phone)
        {
            resident.PhoneId = phone.Id;
            resident.DateRegistration = DateTime.Now;
            resident.DateChange = DateTime.Now;
            resident.ResidentStatus = "На рассмотрении";
            return resident;
        }
    }
}
