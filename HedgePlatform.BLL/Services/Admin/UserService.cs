﻿using AutoMapper;
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
        private IUnitOfWork _db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<UserService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<UserDTO, User>();
            cfg.CreateMap<User, UserDTO>();
        }).CreateMapper();

        public UserDTO GetUser(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var user = _db.Users.Get(id.Value);
            if (user == null)
                throw new ValidationException("NOT_FOUND", "");

            return _mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return _mapper.Map<IEnumerable<User>, List<UserDTO>>(_db.Users.GetAll());
        }

        public void CreateUser(UserDTO user)
        {
            try
            {
                _db.Users.Create(_mapper.Map<UserDTO, User>(user));
                _db.Save();
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

            try
            {
                _db.Users.Update(_mapper.Map<UserDTO, User>(user));
                _db.Save();
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
