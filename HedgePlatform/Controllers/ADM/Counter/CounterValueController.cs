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
    public class CounterValueController : ControllerBase
    {
        ICounterValueService counterValueService;
        public CounterValueController(ICounterValueService service)
        {
            counterValueService = service;
        }
        [HttpGet]
        public IEnumerable<CounterValueViewModel> Index()
        {
            IEnumerable<CounterValueDTO> counterValueDTOs = counterValueService.GetCounterValues();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CounterValueDTO, CounterValueViewModel>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
                cfg.CreateMap<CounterDTO, CounterViewModel>();
            }).CreateMapper();

            var counterValues = mapper.Map<IEnumerable<CounterValueDTO>, List<CounterValueViewModel>>(counterValueDTOs);
            return counterValues;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterValueViewModel counterValue)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterValueViewModel, CounterValueDTO>()).CreateMapper();
            var counterValueDTO = mapper.Map<CounterValueViewModel, CounterValueDTO>(counterValue);
            try
            {
                counterValueService.CreateCounterValue(counterValueDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CounterValueViewModel counterValue)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterValueViewModel, CounterValueDTO>()).CreateMapper();
            var counterValueDTO = mapper.Map<CounterValueViewModel, CounterValueDTO>(counterValue);
            try
            {
                counterValueService.EditCounterValue(counterValueDTO);
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
                counterValueService.DeleteCounterValue(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}