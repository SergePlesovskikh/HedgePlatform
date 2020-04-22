using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    public class ConfirmationController : ControllerBase
    {
        private ICheckService _checkService;
        public ConfirmationController (ICheckService checkService)
        {
            _checkService = checkService;
        }
        [HttpGet]
        public ActionResult<string> Confirmation(string phone, int checkcode, string token)
        {
            try
            {               
                string reg_stat = _checkService.Confirmation(token, checkcode, phone);
                return reg_stat switch
                {
                    "INVALID_TOKEN" => Forbid(reg_stat),
                    "INVALID_CHECK_CODE" => UnprocessableEntity(reg_stat),
                    _ => Ok(reg_stat),
                };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}