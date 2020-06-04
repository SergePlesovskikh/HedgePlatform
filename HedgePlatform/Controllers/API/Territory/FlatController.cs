using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API.Territory
{
    [Route("api/mobile/registration/[controller]")]
    [ApiController]
    public class FlatController : Controller
    {
        private IFlatService _flatService;
        public FlatController(IFlatService flatService)
        {
            _flatService = flatService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => { cfg.CreateMap<FlatDTO, FlatViewModel>();}).CreateMapper();

        [HttpGet]
        public IEnumerable<FlatViewModel> Index(int? HouseId)
        {
            IEnumerable<FlatDTO> flatDTOs = _flatService.GetFlats(HouseId);
            var flats = _mapper.Map<IEnumerable<FlatDTO>, List<FlatViewModel>>(flatDTOs);
            return flats;
        }

        protected override void Dispose(bool disposing)
        {
            _flatService.Dispose();
            base.Dispose(disposing);
        }
    }
}