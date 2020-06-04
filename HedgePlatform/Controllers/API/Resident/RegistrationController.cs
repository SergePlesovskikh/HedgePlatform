using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.ViewModel.API;
using Microsoft.AspNetCore.Mvc;

namespace HedgePlatform.Controllers.API
{
    
    [ApiController]
    public class RegistrationController : Controller
    {
        private IResidentService _residentService;
        public RegistrationController(IResidentService residentService)
        {
            _residentService = residentService;
        }

        [Route("api/mobile/regist/[controller]")]
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

        [Route("api/mobile/work/[controller]")]
        [HttpGet]
        public FileContentResult RequestForm()
        {
            return File(_residentService.GetRequest((int)HttpContext.Items["ResidentId"]), "application/pdf");
        }

        protected override void Dispose(bool disposing)
        {
            _residentService.Dispose();
            base.Dispose(disposing);
        }
    }
}