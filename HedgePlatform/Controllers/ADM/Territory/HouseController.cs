using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM
{
    [Route("api/territory/[controller]")]
    [ApiController]
    public class HouseController : Controller
    {
        private IHouseService _houseService;
        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<HouseDTO, HouseViewModel>().ForMember(s => s.HouseManager, h => h.MapFrom(src => src.HouseManager));
            cfg.CreateMap<HouseManagerDTO, HouseManagerViewModel>();
            cfg.CreateMap<HouseViewModel, HouseDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<HouseViewModel> Index()
        {
            IEnumerable<HouseDTO> houseDTOs = _houseService.GetHouses();            
            var houses = _mapper.Map<IEnumerable<HouseDTO>, List<HouseViewModel>>(houseDTOs);
            return houses;
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseViewModel house)
        {
            var houseDTO = _mapper.Map<HouseViewModel, HouseDTO>(house);
            try
            {
                _houseService.CreateHouse(houseDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] HouseViewModel house)
        {
            var houseDTO = _mapper.Map<HouseViewModel, HouseDTO>(house);
            try
            {
                _houseService.EditHouse(houseDTO);
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
                _houseService.DeleteHouse(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _houseService.Dispose();
            base.Dispose(disposing);
        }
    }
}