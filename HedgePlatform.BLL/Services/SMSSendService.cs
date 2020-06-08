using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HedgePlatform.BLL.Services
{
    public class SMSSendService : ISMSSendService
    {
        private IConfiguration _configuration;
        private ICheckService _checkService;
        private ITokenService _tokenService;
        private readonly ILogger _logger = Log.CreateLogger<ResidentService>();

        public string token { get; set; }
        private static int Checkcode;       
        private string message { get { return "Ваш проверочный код: " + CheckCodeGenerate().ToString(); }  }

        public SMSSendService (IConfiguration configuration, ICheckService checkService, ITokenService tokenService)
        {
            _configuration = configuration;
            _checkService = checkService;
            _tokenService = tokenService;
        }

        public async Task SendSMS(string phone)
        {
            string request_path = RequestPathBuilder();
            string data = DataBuilder(phone);
            //   HttpResponseMessage response = await SendRequest(request_path, data);
            //   if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //   {
            token = _tokenService.GenerateToken();
                _checkService.CreateCheck(new CheckDTO { CheckCode = Checkcode, Phone = phone, SendTime = DateTime.Now, token = token});
          //  }
          /*  else
            {
                _logger.LogError("SMS sending error. Sms server status code: " + response.StatusCode);
                throw new ValidationException("SMS_SEND_FAIL", "");
            }*/
        }

        private string RequestPathBuilder()
        {
            string request_path = "https://";
            request_path += _configuration["SMSSender:smsserver"];          
            return request_path;
        }

        private string DataBuilder(string phone)
        {
            JObject data = new JObject{
                { "message", message },
                { "from", _configuration["SMSSender:sms_name"] },
                { "to", long.Parse(phone)}
            };
            return data.ToString();
        }

        private static int CheckCodeGenerate()
        {
            var rnd = new Random();
            Checkcode = rnd.Next(100000, 999999);
            return Checkcode;
        }

        private async Task<HttpResponseMessage> SendRequest (string request_path, string data)
        {
            using (var client = new HttpClient())
            {
                _logger.LogInformation("Send to sms server: " + request_path);
                client.DefaultRequestHeaders.Authorization =
                                      new AuthenticationHeaderValue(
                                       "Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_configuration["SMSSender:sms_id"])));
             
                _logger.LogInformation("output string: " + data);
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(request_path, content);
                _logger.LogInformation("Response " + response.StatusCode.ToString());
                _logger.LogInformation("Response from SMS server: " + await response.Content.ReadAsStringAsync());
                return response;
            }
        }
    } 
}
