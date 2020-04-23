using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken();
        public string GenerateUid();
    }
}
