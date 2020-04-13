using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;


namespace HedgePlatform.Controllers.ADM
{
    [Route("api/counter/[controller]")]
    [ApiController]
    public class CounterStatusController : ControllerBase
    {
        ICounterStatusService counterStatusService;
        public CounterStatusController(ICounterStatusService service)
        {
            counterStatusService = service;
        }
        [HttpGet]
        public IEnumerable<CounterStatusViewModel> Index()
        {
            IEnumerable<CounterStatusDTO> counterStatusDTOs = counterStatusService.GetCounterStats();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatusDTO, CounterStatusViewModel>()).CreateMapper();
            var counterStats = mapper.Map<IEnumerable<CounterStatusDTO>, List<CounterStatusViewModel>>(counterStatusDTOs);
            return counterStats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterStatusViewModel counterStatus)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatusViewModel, CounterStatusDTO>()).CreateMapper();
            var counterStatusDTO = mapper.Map<CounterStatusViewModel, CounterStatusDTO>(counterStatus);
            try
            {
                counterStatusService.CreateCounterStatus(counterStatusDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CounterStatusViewModel counterStatus)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterStatusViewModel, CounterStatusDTO>()).CreateMapper();
            var counterStatusDTO = mapper.Map<CounterStatusViewModel, CounterStatusDTO>(counterStatus);
            try
            {
                counterStatusService.EditCounterStatus(counterStatusDTO);
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
                counterStatusService.DeleteCounterStatus(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
