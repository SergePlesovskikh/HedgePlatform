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
    public class RegistrationController : ControllerBase
    {
        private IResidentService _residentService;
        public RegistrationController(IResidentService residentService)
        {
            _residentService = residentService;
        }
        [HttpPost]
        public ActionResult<string> Registration([FromBody] ResidentViewModel resident, string uid)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentViewModel, ResidentDTO>()).CreateMapper();
                var residentDTO = mapper.Map<ResidentViewModel, ResidentDTO>(resident);
                _residentService.RegistrationResident(uid, residentDTO);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}