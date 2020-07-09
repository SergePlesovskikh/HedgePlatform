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
    public class CounterTypeController : Controller
    {
        ICounterTypeService _counterTypeService;
        private static IMapper _mapper = new MapperConfiguration(cfg => { 
            cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
            cfg.CreateMap<CounterTypeViewModel, CounterTypeDTO>();
        }).CreateMapper(); 

        public CounterTypeController(ICounterTypeService counterTypeService)
        {
            _counterTypeService = counterTypeService;
        }
     
        [HttpGet]
        public IEnumerable<CounterTypeViewModel> Index()
        {
            IEnumerable<CounterTypeDTO> counterTypeDTOs = _counterTypeService.GetCounterTypes();          
            var counterTypes = _mapper.Map<IEnumerable<CounterTypeDTO>, List<CounterTypeViewModel>>(counterTypeDTOs);
            return counterTypes;
        }
     
        [HttpPost]
        public IActionResult Create([FromBody] CounterTypeViewModel counterType)
        {          
            var counterTypeDTO = _mapper.Map<CounterTypeViewModel, CounterTypeDTO>(counterType);
            try
            {
                _counterTypeService.CreateCounterTypes(counterTypeDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CounterTypeViewModel counterType)
        {
            var counterTypeDTO = _mapper.Map<CounterTypeViewModel, CounterTypeDTO>(counterType);
            try
            {
                _counterTypeService.EditCounterTypes(counterTypeDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                _counterTypeService.DeleteCounterType(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            _counterTypeService.Dispose();
            base.Dispose(disposing);
        }
    }
}
