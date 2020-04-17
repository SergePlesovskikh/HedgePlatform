using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Counter
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        ICounterService counterService;
        public CounterController(ICounterService service)
        {
            counterService = service;
        }
        [HttpGet]
        public IEnumerable<CounterViewModel> Index()
        {
            IEnumerable<CounterDTO> counterDTOs = counterService.GetCounters();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CounterDTO, CounterViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
                .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
                .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
                cfg.CreateMap<FlatDTO, FlatViewModel>();
                cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
                cfg.CreateMap<CounterStatusDTO, CounterStatusViewModel>();
            }).CreateMapper();

            var counters = mapper.Map<IEnumerable<CounterDTO>, List<CounterViewModel>>(counterDTOs);
            return counters;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterViewModel counter)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterViewModel, CounterDTO>()).CreateMapper();
            var counterDTO = mapper.Map<CounterViewModel, CounterDTO>(counter);
            try
            {
                counterService.CreateCounter(counterDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CounterViewModel counter)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterViewModel, CounterDTO>()).CreateMapper();
            var counterDTO = mapper.Map<CounterViewModel, CounterDTO>(counter);
            try
            {
                counterService.EditCounter(counterDTO);
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
                counterService.DeleteCounter(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}