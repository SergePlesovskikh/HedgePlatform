using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Counter
{
    [Route("api/counter/[controller]")]
    [ApiController]
    public class CounterTypeController : ControllerBase
    {
        ICounterTypeService counterTypeService;
        public CounterTypeController(ICounterTypeService service)
        {
            counterTypeService = service;
        }
     
        [HttpGet]
        public IEnumerable<CounterTypeViewModel> Index()
        {
            IEnumerable<CounterTypeDTO> counterTypeDTOs = counterTypeService.GetCounterTypes();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>()).CreateMapper();
            var counterTypes = mapper.Map<IEnumerable<CounterTypeDTO>, List<CounterTypeViewModel>>(counterTypeDTOs);
            return counterTypes;
        }
     
        [HttpPost]
        public IActionResult Create([FromBody] CounterTypeViewModel counterType)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterTypeViewModel, CounterTypeDTO>()).CreateMapper();
            var counterTypeDTO = mapper.Map<CounterTypeViewModel, CounterTypeDTO>(counterType);
            try
            {
                counterTypeService.CreateCounterTypes(counterTypeDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CounterTypeViewModel counterType)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterTypeViewModel, CounterTypeDTO>()).CreateMapper();
            var counterTypeDTO = mapper.Map<CounterTypeViewModel, CounterTypeDTO>(counterType);
            try
            {
                counterTypeService.EditCounterTypes(counterTypeDTO);
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
                counterTypeService.DeleteCounterType(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
