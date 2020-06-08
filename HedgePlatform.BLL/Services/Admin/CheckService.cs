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
    public class CheckService : ICheckService
    {
        private IUnitOfWork _db { get; set; }
        private ISessionService _sessionService;
        private IPhoneService _phoneService;
        private ITokenService _tokenService;

        public CheckService(IUnitOfWork uow, ISessionService sessionService, IPhoneService phoneService, ITokenService tokenService)
        {
            _db = uow;
            _sessionService = sessionService;
            _phoneService = phoneService;
            _tokenService = tokenService;
        }

        private readonly ILogger _logger = Log.CreateLogger<CheckService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<CheckDTO, Check>()).CreateMapper();

        public string Confirmation(string token, int checkcode, string phone_number)
        {
            string conf_stat = "INVALID_TOKEN";
           
            Check check = _db.Checks.FindFirst(x => x.token == token);
            if (check!=null)
            {
                if (check.CheckCode==checkcode)
                {
                    PhoneDTO phone = _phoneService.GetOrCreate(phone_number);
                    if (phone != null)
                    {
                        SessionDTO session = _sessionService.CreateSession(new SessionDTO { PhoneId = phone.Id, Uid = _tokenService.GenerateUid() });
                        conf_stat = session.Uid;
                        DeleteCheck(check.Id);   
                    }
                    else conf_stat = "INVALID_PHONE_NUMBER";
                }
                else conf_stat = "INVALID_CHECK_CODE";   
            }            
            return conf_stat;
        }

        public void CreateCheck(CheckDTO check)
        {           
            try
            {
                _db.Checks.Create(_mapper.Map<CheckDTO, Check>(check));
                _db.Save();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }
            catch (Exception ex)
            {
                _logger.LogError("Check creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }      

        public void DeleteCheck(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var check = _db.Checks.Get(id.Value);
            if (check == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Checks.Delete(id.Value);
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
