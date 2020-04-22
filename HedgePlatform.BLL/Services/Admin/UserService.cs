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
    public class UserService : IUserService
    {
        IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<UserService>();

        public UserDTO GetUser(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var user = db.Users.Get(id.Value);
            if (user == null)
                throw new ValidationException("NOT_FOUND", "");

            return new UserDTO { Id = user.Id, Login = user.Login, Psw = user.Psw };
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(db.Users.GetAll());
        }

        public void CreateUser(UserDTO user)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            try
            {
                db.Users.Create(mapper.Map<UserDTO, User>(user));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("User creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditUser(UserDTO user)
        {
            if (user == null)
                throw new ValidationException("No user object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            try
            {
                db.Users.Update(mapper.Map<UserDTO, User>(user));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("User edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("User edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void DeleteUser(int? id)
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
