using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Resident
{
    [Route("api/resident/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        IResidentService residentService;
        public ResidentController(IResidentService service)
        {
            residentService = service;
        }
        [HttpGet]
        public IEnumerable<ResidentViewModel> Index()
        {
            IEnumerable<ResidentDTO> residentDTOs = residentService.GetResidents();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<ResidentDTO, ResidentViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat));
                cfg.CreateMap<FlatDTO, FlatViewModel>();
            }).CreateMapper();

            var residents = mapper.Map<IEnumerable<ResidentDTO>, List<ResidentViewModel>>(residentDTOs);
            return residents;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ResidentViewModel resident)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentViewModel, ResidentDTO>()).CreateMapper();
            var residentDTO = mapper.Map<ResidentViewModel, ResidentDTO>(resident);
            try
            {
                residentService.CreateResident(residentDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] ResidentViewModel resident)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ResidentViewModel, ResidentDTO>()).CreateMapper();
            var residentDTO = mapper.Map<ResidentViewModel, ResidentDTO>(resident);
            try
            {
                residentService.EditResident(residentDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                residentService.DeleteResident(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}