using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HedgePlatform.BLL.Interfaces
{
    public interface ISMSSendService
    {
        public Task SendSMS(string phone);
        string token { get; }
    }
}
