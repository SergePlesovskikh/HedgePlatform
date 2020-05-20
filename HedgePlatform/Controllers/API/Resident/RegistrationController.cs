using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HedgePlatform.Controllers.API
{
    
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IResidentService _residentService;
        public RegistrationController(IResidentService residentService)
        {
            _residentService = residentService;
        }

        [Route("api/auth/[controller]")]
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

        [Route("api/mobile/[controller]")]
        [HttpGet]
        public FileContentResult RequestForm()
        {
            return File(_residentService.GetRequest((int)HttpContext.Items["ResidentId"]), "application/pdf");
        }
    }
}