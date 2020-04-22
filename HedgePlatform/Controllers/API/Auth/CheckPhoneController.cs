using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using System.Threading.Tasks;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    public class CheckPhoneController : ControllerBase
    {
        private ISMSSendService _smsSendService;
        private IPhoneService _phoneService;
        public CheckPhoneController (ISMSSendService smsSendService, IPhoneService phoneService)
        {
            _smsSendService = smsSendService;
            _phoneService = phoneService;
        }
        //TODO проверка корректности номера
        [HttpGet]
        public async Task<ActionResult<string>> Get (string phone)
        {
            try
            {   if (_phoneService.CheckPhone(phone))
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
                return BadRequest(ex.Message);
            }
        }
    }
}