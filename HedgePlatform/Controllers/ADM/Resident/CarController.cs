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
    public class CarController : ControllerBase
    {
        ICarService carService;
        public CarController(ICarService service)
        {
            carService = service;
        }
        [HttpGet]
        public IEnumerable<CarViewModel> Index()
        {
            IEnumerable<CarDTO> carDTOs = carService.GetCars();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CarDTO, CarViewModel>().ForMember(s => s.Flat, h => h.MapFrom(src => src.flat));
                cfg.CreateMap<FlatDTO, FlatViewModel>();
            }).CreateMapper();

            var cars = mapper.Map<IEnumerable<CarDTO>, List<CarViewModel>>(carDTOs);
            return cars;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CarViewModel car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>()).CreateMapper();
            var carDTO = mapper.Map<CarViewModel, CarDTO>(car);
            try
            {
                carService.CreateCar(carDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] CarViewModel car)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>()).CreateMapper();
            var carDTO = mapper.Map<CarViewModel, CarDTO>(car);
            try
            {
                carService.EditCar(carDTO);
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
                carService.DeleteCar(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}