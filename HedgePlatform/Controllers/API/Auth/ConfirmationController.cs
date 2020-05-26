using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/auth/[controller]")]
    [ApiController]
    public class ConfirmationController : ControllerBase
    {
        private ICheckService _checkService;
        public ConfirmationController (ICheckService checkService)
        {
            _checkService = checkService;
        }
        [HttpGet]
        public ActionResult<string> Confirmation(int checkcode, string token, string phone)
        {
            try
            {               
                string conf_stat = _checkService.Confirmation(token, checkcode, phone);
                return conf_stat switch
                {
                    "INVALID_TOKEN" => Forbid(conf_stat),
                    "INVALID_CHECK_CODE" => UnprocessableEntity(conf_stat),
                    "INVALID_PHONE_NUMBER" => NotFound(conf_stat),
                    _ => Ok(conf_stat)
                };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}