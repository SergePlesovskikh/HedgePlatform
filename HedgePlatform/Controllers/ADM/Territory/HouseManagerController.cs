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
    public class HouseManagerController : ControllerBase
    {
        IHouseManagerService houseManagerService;
        public HouseManagerController(IHouseManagerService service)
        {
            houseManagerService = service;
        }

        [HttpGet]
        public IEnumerable<HouseManagerViewModel> Index()
        {
            IEnumerable<HouseManagerDTO> houseManagerDTOs = houseManagerService.GetHouseManagers();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManagerDTO, HouseManagerViewModel>()).CreateMapper();
            var houseManagers = mapper.Map<IEnumerable<HouseManagerDTO>, List<HouseManagerViewModel>>(houseManagerDTOs);
            return houseManagers;
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseManagerViewModel houseManager)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManagerViewModel, HouseManagerDTO>()).CreateMapper();
            var houseManagerDTO = mapper.Map<HouseManagerViewModel, HouseManagerDTO>(houseManager);
            try
            {
                houseManagerService.CreateHouseManager(houseManagerDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] HouseManagerViewModel houseManager)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseManagerViewModel, HouseManagerDTO>()).CreateMapper();
            var houseManagerDTO = mapper.Map<HouseManagerViewModel, HouseManagerDTO>(houseManager);
            try
            {
                houseManagerService.EditHouseManager(houseManagerDTO);
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
                houseManagerService.DeleteHouseManager(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}