﻿using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;

namespace HedgePlatform.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        ICounterService counterService;
        public CounterController(ICounterService service)
        {
            counterService = service;
        }

        [HttpGet]
        public IEnumerable<CounterViewModel> Index()
        {
            IEnumerable<CounterDTO> counterDTOs = counterService.GetCounters();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<CounterDTO, CounterViewModel>()
                .ForMember(s => s.CounterType, h => h.MapFrom(src => src.CounterType));       
               
             //   cfg.CreateMap<CounterTypeDTO, CounterTypeViewModel>();
             //   cfg.CreateMap<CounterStatusDTO, CounterStatusViewModel>();
            }).CreateMapper();

            var counters = mapper.Map<IEnumerable<CounterDTO>, List<CounterViewModel>>(counterDTOs);
            return counters;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CounterViewModel counter)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CounterViewModel, CounterDTO>()).CreateMapper();
            var counterDTO = mapper.Map<CounterViewModel, CounterDTO>(counter);
            try
            {
                counterService.CreateCounter(counterDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}