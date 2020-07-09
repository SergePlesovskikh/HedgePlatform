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
    public class ResidentController : Controller
    {
        private IResidentService _residentService;
        public ResidentController(IResidentService residentService)
        {
            _residentService = residentService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<ResidentDTO, ResidentViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat));
            cfg.CreateMap<FlatDTO, FlatViewModel>();
            cfg.CreateMap<ResidentViewModel, ResidentDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<ResidentViewModel> Index()
        {
            IEnumerable<ResidentDTO> residentDTOs = _residentService.GetResidents();
            var residents = _mapper.Map<IEnumerable<ResidentDTO>, List<ResidentViewModel>>(residentDTOs);
            return residents;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ResidentViewModel resident)
        {            
            var residentDTO =_mapper.Map<ResidentViewModel, ResidentDTO>(resident);
            try
            {
                _residentService.CreateResident(residentDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] ResidentViewModel resident)
        {
            var residentDTO = _mapper.Map<ResidentViewModel, ResidentDTO>(resident);
            try
            {
                _residentService.EditResident(residentDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _residentService.DeleteResident(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _residentService.Dispose();
            base.Dispose(disposing);
        }
    }
}