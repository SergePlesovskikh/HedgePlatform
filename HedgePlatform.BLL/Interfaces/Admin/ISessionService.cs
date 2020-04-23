using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ISessionService
    {
        SessionDTO GetSession(int? id);
        SessionDTO GetSession(string uid);
        SessionDTO CreateSession(SessionDTO session);
        void DeleteSession(int? id);
        void Dispose();
    }
}
