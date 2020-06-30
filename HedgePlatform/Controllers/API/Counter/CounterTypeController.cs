using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class CounterTypeController : Controller
    {
        private ICounterTypeService _counterTypeService;
        public CounterTypeController(ICounterTypeService counterTypeService)
        {
            _counterTypeService = counterTypeService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CounterTypeViewModel> Index()
        {
            IEnumerable<CounterTypeDTO> counterTypeDTOs = _counterTypeService.GetCounterTypes();
            var countersTypes = _mapper.Map<IEnumerable<CounterTypeDTO>, List<CounterTypeViewModel>>(counterTypeDTOs);
            return countersTypes;
        }

        protected override void Dispose(bool disposing)
        {
            _counterTypeService.Dispose();
            base.Dispose(disposing);
        }
    }
}