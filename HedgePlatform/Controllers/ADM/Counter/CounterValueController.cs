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
    public class CounterValueController : Controller
    {
        private ICounterValueService _counterValueService;
        public CounterValueController(ICounterValueService counterValueService)
        {
            _counterValueService = counterValueService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterValueDTO, CounterValueViewModel>().ForMember(s => s.Counter, h => h.MapFrom(src => src.Counter));
            cfg.CreateMap<CounterDTO, CounterViewModel>();
            cfg.CreateMap<CounterValueViewModel, CounterValueDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterValueViewModel> Index()
        {
            IEnumerable<CounterValueDTO> counterValueDTOs = _counterValueService.GetCounterValues();
            var counterValues = _mapper.Map<IEnumerable<CounterValueDTO>, List<CounterValueViewModel>>(counterValueDTOs);
            return counterValues;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterValueViewModel counterValue)
        {           
            var counterValueDTO = _mapper.Map<CounterValueViewModel, CounterValueDTO>(counterValue);
            try
            {
                _counterValueService.CreateCounterValue(counterValueDTO);
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
            var counterValueDTO = _mapper.Map<CounterValueViewModel, CounterValueDTO>(counterValue);
            try
            {
                _counterValueService.EditCounterValue(counterValueDTO);
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
                _counterValueService.DeleteCounterValue(id);
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