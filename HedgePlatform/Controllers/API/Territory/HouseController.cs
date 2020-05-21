using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private IHouseService _houseService;
        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpGet]
        public IEnumerable<HouseViewModel> Index()
        {
            IEnumerable<HouseDTO> houseDTOs = _houseService.GetHouses();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<HouseDTO, HouseViewModel>();
            }).CreateMapper();

            var houses = mapper.Map<IEnumerable<HouseDTO>, List<HouseViewModel>>(houseDTOs);
            return houses;
        }
    }
}