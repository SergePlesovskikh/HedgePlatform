using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Territory
{
    [Route("api/territory/[controller]")]
    [ApiController]
    public class FlatController : ControllerBase
    {
        IFlatService flatService;
        public FlatController(IFlatService service)
        {
            flatService = service;
        }
        [HttpGet]
        public IEnumerable<FlatViewModel> Index()
        {
            IEnumerable<FlatDTO> flatDTOs = flatService.GetFlats();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<FlatDTO, FlatViewModel>().ForMember(s => s.House, h => h.MapFrom(src => src.House));
                cfg.CreateMap<HouseDTO, HouseViewModel>();
            }).CreateMapper();

            var flats = mapper.Map<IEnumerable<FlatDTO>, List<FlatViewModel>>(flatDTOs);
            return flats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] FlatViewModel flat)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FlatViewModel, FlatDTO>()).CreateMapper();
            var flatDTO = mapper.Map<FlatViewModel, FlatDTO>(flat);
            try
            {
                flatService.CreateFlat(flatDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] FlatViewModel flat)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FlatViewModel, FlatDTO>()).CreateMapper();
            var flatDTO = mapper.Map<FlatViewModel, FlatDTO>(flat);
            try
            {
                flatService.EditFlat(flatDTO);
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
                flatService.DeleteFlat(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}