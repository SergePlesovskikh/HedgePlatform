using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using System.Threading.Tasks;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/auth/[controller]")]
    [ApiController]
    public class CheckPhoneController : Controller
    {
        private ISMSSendService _smsSendService;
        private IPhoneService _phoneService;
        public CheckPhoneController (ISMSSendService smsSendService, IPhoneService phoneService)
        {
            _smsSendService = smsSendService;
            _phoneService = phoneService;
        }
        //ToDo проверка корректности номера в BLL
        [HttpGet]
        public async Task<IActionResult> Get (string phone)
        {
            try
            {   
                if (_phoneService.CheckPhone(phone))
                {
                    await _smsSendService.SendSMS(phone);
                    return Ok(_smsSendService.token);
                }
                else
                {
                    return Ok("ALREADY");
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _phoneService.Dispose();
            base.Dispose(disposing);
        }
    }
}