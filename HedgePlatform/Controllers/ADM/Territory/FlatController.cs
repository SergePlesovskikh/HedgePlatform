using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Territory
{
    [Route("api/territory/[controller]")]
    [ApiController]
    public class FlatController : Controller
    {
        private IFlatService _flatService;
        public FlatController(IFlatService flatService)
        {
            _flatService = flatService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<FlatDTO, FlatViewModel>().ForMember(s => s.House, h => h.MapFrom(src => src.House));
            cfg.CreateMap<HouseDTO, HouseViewModel>();
            cfg.CreateMap<FlatViewModel, FlatDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<FlatViewModel> Index()
        {
            IEnumerable<FlatDTO> flatDTOs = _flatService.GetFlats();
            var flats = _mapper.Map<IEnumerable<FlatDTO>, List<FlatViewModel>>(flatDTOs);
            return flats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] FlatViewModel flat)
        {
            var flatDTO = _mapper.Map<FlatViewModel, FlatDTO>(flat);
            try
            {
                _flatService.CreateFlat(flatDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] FlatViewModel flat)
        {
            var flatDTO = _mapper.Map<FlatViewModel, FlatDTO>(flat);
            try
            {
                _flatService.EditFlat(flatDTO);
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
                _flatService.DeleteFlat(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            _flatService.Dispose();
            base.Dispose(disposing);
        }
    }
}