using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class CounterValueController : Controller
    {
        private ICounterValueService _counterValueService;
        public CounterValueController(ICounterValueService service)
        {
            _counterValueService = service;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterValueDTO, CounterValueViewModel>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
            cfg.CreateMap<CounterDTO, CounterViewModel>();
            cfg.CreateMap<CounterValueViewModel, CounterValueDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterValueViewModel> Index(int CounterId)
        {
            IEnumerable<CounterValueDTO> counterValueDTOs = _counterValueService.GetCounterValuesByCounter(CounterId);
            var counterValues = _mapper.Map<IEnumerable<CounterValueDTO>, List<CounterValueViewModel>>(counterValueDTOs);
            return counterValues;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterValueViewModel counterValue)
        {
            var counterValueDTO = _mapper.Map<CounterValueViewModel, CounterValueDTO>(counterValue);
            try
            {
                _counterValueService.CreateCounterValue(counterValueDTO, (int)HttpContext.Items["FlatId"]);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            _counterValueService.Dispose();
            base.Dispose(disposing);
        }
    }
}