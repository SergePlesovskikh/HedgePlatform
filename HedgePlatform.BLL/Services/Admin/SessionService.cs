using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.BLL.Services.Admin
{
    public class SessionService : ISessionService
    {
        IUnitOfWork db { get; set; }

        public SessionService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<SessionService>();

        public SessionDTO GetSession(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var session = db.Sessions.Get(id.Value);
            if (session == null)
                throw new ValidationException("NOT_FOUND", "");

            return new SessionDTO { Id = session.Id, Uid = session.Uid, PhoneId = session.PhoneId };
        }
        //ToDo добавить include для 1 объекта
        public SessionDTO GetSession(string uid)
        {
            if (uid == null)
                throw new ValidationException("NULL", "");
            var session = db.Sessions.FindFirst(x => x.Uid == uid);
            if (session == null)
                throw new ValidationException("NOT_FOUND", "");

            return new SessionDTO { Id = session.Id, Uid = session.Uid, PhoneId = session.PhoneId };
        }

        public SessionDTO CreateSession(SessionDTO session)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SessionDTO, Session>()).CreateMapper();
            try
            {
                Session new_session = db.Sessions.Create(mapper.Map<SessionDTO, Session>(session));
                db.Save();
                return mapper.Map<Session, SessionDTO>(new_session);
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
            var counterType = db.CounterStats.Get(id.Value);
            if (counterType == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.CounterStats.Delete(id.Value);
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
