using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.ADM
{
    [Route("api/inform/[controller]")]
    [ApiController]
    public class VoteResultController : ControllerBase
    {
        IVoteResultService voteResultService;
        public VoteResultController(IVoteResultService service)
        {
            voteResultService = service;
        }
        [HttpGet]
        public IEnumerable<VoteResultViewModel> Index()
        {
            IEnumerable<VoteResultDTO> voteResultDTOs = voteResultService.GetVoteResults();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteResultDTO, VoteResultViewModel>().ForMember(s => s.VoteOption, h => h.MapFrom(src => src.VoteOption))
                .ForMember(s => s.Resident, h => h.MapFrom(src => src.Resident));
                cfg.CreateMap<ResidentDTO, ResidentViewModel>();
                cfg.CreateMap<VoteOptionDTO, VoteOptionViewModel>();
            }).CreateMapper();

            var voteResults = mapper.Map<IEnumerable<VoteResultDTO>, List<VoteResultViewModel>>(voteResultDTOs);
            return voteResults;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultViewModel, VoteResultDTO>()).CreateMapper();
            var voteResultDTO = mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                voteResultService.CreateVoteResult(voteResultDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] VoteResultViewModel voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultViewModel, VoteResultDTO>()).CreateMapper();
            var voteResultDTO = mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                voteResultService.EditVoteResult(voteResultDTO);
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
                voteResultService.DeleteVoteResult(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}