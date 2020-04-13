using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System.Collections.Generic;

namespace HedgePlatform.BLL.Services
{
    public class ResidentService : IResidentService
    {
        IUnitOfWork db { get; set; }

        public ResidentService(IUnitOfWork uow)
        {
            db = uow;
        }

        public ResidentDTO GetResident(int? id)
        {
            if (id == null)
                throw new ValidationException("Resident id is null", "");
            var resident = db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("resident is not found", "");

            return new ResidentDTO { Id = resident.Id, BirthDate = resident.BirthDate, DateChange = resident.DateChange, DateRegistration = resident.DateRegistration,
                                    FIO = resident.FIO, FlatId = resident.FlatId, Phone = resident.Phone
            };
        }

        public IEnumerable<ResidentDTO> GetResident()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Resident, ResidentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Resident>, List<ResidentDTO>>(db.Residents.GetAll());
        }

        public void CreateResident(ResidentDTO resident)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentDTO, Resident>()).CreateMapper();
            try
            {
                db.Residents.Create(mapper.Map<ResidentDTO, Resident>(resident));
                db.Save();
            }

            catch
            {
                throw new ValidationException("Error for creating resident", "");
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
            }

            catch
            {
                throw new ValidationException("Error for editing Resident", "");
            }
        }
        public void DeleteResident(int? id)
        {
            if (id == null)
                throw new ValidationException("Resident id is null", "");
            var resident = db.Residents.Get(id.Value);
            if (resident == null)
                throw new ValidationException("Resident is not found", "");
            try
            {
                db.Residents.Delete(id.Value);
                db.Save();
            }
            catch
            {
                throw new ValidationException("Error for editing Resident", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}
