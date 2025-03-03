﻿using HedgePlatform.BLL.DTO;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ICheckService
    {      
        string Confirmation (string token, int checkcode, string phone);  
        void CreateCheck(CheckDTO check);
        void DeleteCheck(int? id);
        void Dispose();
    }
}
