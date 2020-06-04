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
    public class CounterController : Controller
    {
        private ICounterService _counterService;
        public CounterController(ICounterService counterService)
        {
            _counterService = counterService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterDTO, CounterViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.Flat))
            .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType))
            .ForMember(s => s.CounterStatus, h => h.MapFrom(src => src.CounterStatus));
            cfg.CreateMap<FlatDTO, FlatViewModel>();
            cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
            cfg.CreateMap<CounterStatusDTO, CounterStatusViewModel>();
            cfg.CreateMap<CounterViewModel, CounterDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterViewModel> Index()
        {
            IEnumerable<CounterDTO> counterDTOs = _counterService.GetCounters();
            var counters = _mapper.Map<IEnumerable<CounterDTO>, List<CounterViewModel>>(counterDTOs);
            return counters;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterViewModel counter)
        {
            var counterDTO = _mapper.Map<CounterViewModel, CounterDTO>(counter);
            try
            {
                _counterService.CreateCounter(counterDTO);
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
            var counterDTO = _mapper.Map<CounterViewModel, CounterDTO>(counter);
            try
            {
                _counterService.EditCounter(counterDTO);
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
                _counterService.DeleteCounter(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _counterService.Dispose();
            base.Dispose(disposing);
        }
    }
}