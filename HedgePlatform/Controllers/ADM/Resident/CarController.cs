using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM.Resident
{
    [Route("api/resident/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<CarDTO, CarViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.flat));
            cfg.CreateMap<FlatDTO, FlatViewModel>();
            cfg.CreateMap<CarViewModel, CarDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<CarViewModel> Index()
        {
            IEnumerable<CarDTO> carDTOs = _carService.GetCars();
            var cars = _mapper.Map<IEnumerable<CarDTO>, List<CarViewModel>>(carDTOs);
            return cars;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CarViewModel car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>()).CreateMapper();
            var carDTO = _mapper.Map<CarViewModel, CarDTO>(car);
            try
            {
                _carService.CreateCar(carDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CarViewModel car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>()).CreateMapper();
            var carDTO = _mapper.Map<CarViewModel, CarDTO>(car);
            try
            {
                _carService.EditCar(carDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _carService.DeleteCar(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            _carService.Dispose();
            base.Dispose(disposing);
        }
    }
}