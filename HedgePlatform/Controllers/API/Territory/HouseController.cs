using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/registration/[controller]")]
    [ApiController]
    public class HouseController : Controller
    {
        private IHouseService _houseService;
        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }
        private static IMapper _mapper = new MapperConfiguration(cfg => { cfg.CreateMap<HouseDTO, HouseViewModel>();}).CreateMapper();

        [HttpGet]
        public IEnumerable<HouseViewModel> Index()
        {
            IEnumerable<HouseDTO> houseDTOs = _houseService.GetHouses();
            var houses = _mapper.Map<IEnumerable<HouseDTO>, List<HouseViewModel>>(houseDTOs);
            return houses;
        }

        protected override void Dispose(bool disposing)
        {
            _houseService.Dispose();
            base.Dispose(disposing);
        }
    }
}