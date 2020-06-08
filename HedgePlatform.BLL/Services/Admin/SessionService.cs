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
    public class SessionService : ISessionService
    {
        private IUnitOfWork _db { get; set; }

        public SessionService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<SessionService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Session, SessionDTO>();
            cfg.CreateMap<SessionDTO, Session>();}).CreateMapper();

        public SessionDTO GetSession(string uid)
        {           
            var session = _db.Sessions.FindFirst(x => x.Uid == uid);
            return _mapper.Map<Session, SessionDTO>(session);
        }

        public SessionDTO CreateSession(SessionDTO session)
        {
            try
            {
                Session new_session = _db.Sessions.Create(_mapper.Map<SessionDTO, Session>(session));
                _db.Save();
                return _mapper.Map<Session, SessionDTO>(new_session);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Session creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteSession(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var counterType = _db.CounterStats.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");

            try
            {
                _db.CounterStats.Delete(id.Value);
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
