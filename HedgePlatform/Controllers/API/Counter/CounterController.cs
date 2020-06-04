using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class CounterController : Controller
    {
        private ICounterService _counterService;
        public CounterController(ICounterService counterService)
        {
            _counterService = counterService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterDTO, CounterViewModel>()
            .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType));
            cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
            cfg.CreateMap<CounterViewModel, CounterDTO>();
            cfg.CreateMap<CounterValueViewModel, CounterValueDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterViewModel> Index()
        {

            IEnumerable<CounterDTO> counterDTOs = _counterService.GetCountersByFlat((int)HttpContext.Items["FlatId"]);
            var counters = _mapper.Map<IEnumerable<CounterDTO>, List<CounterViewModel>>(counterDTOs);
            return counters;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterViewModel counter)
        {
            var counterDTO = _mapper.Map<CounterViewModel, CounterDTO>(counter);
            var counterValue = _mapper.Map<CounterValueViewModel, CounterValueDTO>(counter.LastCounterValue);

            try
            {
                _counterService.CreateCounter(counterDTO, counterValue, (int)HttpContext.Items["FlatId"]);
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