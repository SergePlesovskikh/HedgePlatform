using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API.Counter
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class CounterTypeController : ControllerBase
    {
        ICounterTypeService _counterTypeService;
        public CounterTypeController(ICounterTypeService counterTypeService)
        {
            _counterTypeService = counterTypeService;
        }

        [HttpGet]
        public IEnumerable<CounterTypeViewModel> Index()
        {
            IEnumerable<CounterTypeDTO> counterTypeDTOs = _counterTypeService.GetCounterTypes();
            var mapper = new MapperConfiguration(cfg => {                
                cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
            }).CreateMapper();

            var countersTypes = mapper.Map<IEnumerable<CounterTypeDTO>, List<CounterTypeViewModel>>(counterTypeDTOs);
            return countersTypes;
        }
    }
}