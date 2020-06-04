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
    public class CounterStatusController : Controller
    {
        private ICounterStatusService _counterStatusService;
        public CounterStatusController(ICounterStatusService counterStatusService)
        {
            _counterStatusService = counterStatusService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => { 
            cfg.CreateMap<CounterStatusDTO, CounterStatusViewModel>();
            cfg.CreateMap<CounterStatusViewModel, CounterStatusDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterStatusViewModel> Index()
        {
            IEnumerable<CounterStatusDTO> counterStatusDTOs = _counterStatusService.GetCounterStats();           
            var counterStats = _mapper.Map<IEnumerable<CounterStatusDTO>, List<CounterStatusViewModel>>(counterStatusDTOs);
            return counterStats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterStatusViewModel counterStatus)
        {            
            var counterStatusDTO = _mapper.Map<CounterStatusViewModel, CounterStatusDTO>(counterStatus);
            try
            {
                _counterStatusService.CreateCounterStatus(counterStatusDTO);
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
            var counterStatusDTO = _mapper.Map<CounterStatusViewModel, CounterStatusDTO>(counterStatus);
            try
            {
                _counterStatusService.EditCounterStatus(counterStatusDTO);
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
                _counterStatusService.DeleteCounterStatus(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            _counterStatusService.Dispose();
            base.Dispose(disposing);
        }
    }
}
