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
    public class PhoneService
    {
        IUnitOfWork db { get; set; }

        public PhoneService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<UserService>();
        public bool CheckPhone(string phone)
        {
            if (db.Phones.Find(x => x.Number == phone) == null)
                return true;
            else return false;
        }
    }
}
