﻿using AutoMapper;
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
        IUnitOfWork db { get; set; }
        private ISessionService _sessionService;
        private IPhoneService _phoneService;
        private ITokenService _tokenService;

        public CheckService(IUnitOfWork uow, ISessionService sessionService, IPhoneService phoneService, ITokenService tokenService)
        {
            db = uow;
            _sessionService = sessionService;
            _phoneService = phoneService;
            _tokenService = tokenService;
        }

        private readonly ILogger _logger = Log.CreateLogger<CheckService>();

        public CheckDTO GetCheck(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var check = db.Checks.Get(id.Value);
            if (check == null)
                throw new ValidationException("NOT_FOUND", "");

            return new CheckDTO { Id = check.Id, CheckCode = check.CheckCode, Phone = check.Phone, SendTime = check.SendTime, token = check.token };
        }

        public string Confirmation(string token, int checkcode, string phone_number)
        {
            string conf_stat = "INVALID_TOKEN";
           
            Check check = db.Checks.FindFirst(x => x.token == token);
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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CheckDTO, Check>()).CreateMapper();
            try
            {
                db.Checks.Create(mapper.Map<CheckDTO, Check>(check));
                db.Save();
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
            var check = db.Checks.Get(id.Value);
            if (check == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Checks.Delete(id.Value);
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
