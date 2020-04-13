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
    public class HouseController : ControllerBase
    {
        IHouseService houseService;
        public HouseController(IHouseService service)
        {
            houseService = service;
        }
        [HttpGet]
        public IEnumerable<HouseViewModel> Index()
        {
            IEnumerable<HouseDTO> houseDTOs = houseService.GetHouses();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseDTO, HouseViewModel>()).CreateMapper();
            var counterStats = mapper.Map<IEnumerable<HouseDTO>, List<HouseViewModel>>(houseDTOs);
            return counterStats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseViewModel house)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseViewModel, HouseDTO>()).CreateMapper();
            var houseDTO = mapper.Map<HouseViewModel, HouseDTO>(house);
            try
            {
                houseService.CreateHouse(houseDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] HouseViewModel house)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<HouseViewModel, HouseDTO>()).CreateMapper();
            var houseDTO = mapper.Map<HouseViewModel, HouseDTO>(house);
            try
            {
                houseService.EditHouse(houseDTO);
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
                houseService.DeleteHouse(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}