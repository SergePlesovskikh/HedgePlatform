using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API.Territory
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    public class FlatController : ControllerBase
    {
        private IFlatService _flatService;
        public FlatController(IFlatService flatService)
        {
            _flatService = flatService;
        }

        [HttpGet]
        public IEnumerable<FlatViewModel> Index(int? HouseId)
        {
            IEnumerable<FlatDTO> flatDTOs = _flatService.GetFlats(HouseId);

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<FlatDTO, FlatViewModel>();
            }).CreateMapper();

            var flats = mapper.Map<IEnumerable<FlatDTO>, List<FlatViewModel>>(flatDTOs);
            return flats;
        }
    }
}