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
    public class HouseManagerController : Controller
    {
        private IHouseManagerService _houseManagerService;
        public HouseManagerController(IHouseManagerService houseManagerService)
        {
            _houseManagerService = houseManagerService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<HouseManagerDTO, HouseManagerViewModel>();
            cfg.CreateMap<HouseManagerViewModel, HouseManagerDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<HouseManagerViewModel> Index()
        {
            IEnumerable<HouseManagerDTO> houseManagerDTOs = _houseManagerService.GetHouseManagers();
            var houseManagers = _mapper.Map<IEnumerable<HouseManagerDTO>, List<HouseManagerViewModel>>(houseManagerDTOs);
            return houseManagers;
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseManagerViewModel houseManager)
        {
            var houseManagerDTO = _mapper.Map<HouseManagerViewModel, HouseManagerDTO>(houseManager);
            try
            {
                _houseManagerService.CreateHouseManager(houseManagerDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] HouseManagerViewModel houseManager)
        {
            var houseManagerDTO = _mapper.Map<HouseManagerViewModel, HouseManagerDTO>(houseManager);
            try
            {
                _houseManagerService.EditHouseManager(houseManagerDTO);
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
                _houseManagerService.DeleteHouseManager(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            _houseManagerService.Dispose();
            base.Dispose(disposing);
        }
    }
}