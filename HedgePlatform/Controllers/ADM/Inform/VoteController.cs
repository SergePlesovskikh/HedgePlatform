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
    public class VoteController : ControllerBase
    {
        IVoteService voteService;
        public VoteController(IVoteService service)
        {
            voteService = service;
        }
        [HttpGet]
        public IEnumerable<VoteViewModel> Index()
        {
            IEnumerable<VoteDTO> voteDTOs = voteService.GetVotes();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteDTO, VoteViewModel>()).CreateMapper();
            var counterStats = mapper.Map<IEnumerable<VoteDTO>, List<VoteViewModel>>(voteDTOs);
            return counterStats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteViewModel vote)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteViewModel, VoteDTO>()).CreateMapper();
            var voteDTO = mapper.Map<VoteViewModel, VoteDTO>(vote);
            try
            {
                voteService.CreateVote(voteDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] VoteViewModel vote)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteViewModel, VoteDTO>()).CreateMapper();
            var voteDTO = mapper.Map<VoteViewModel, VoteDTO>(vote);
            try
            {
                voteService.EditVote(voteDTO);
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
                voteService.DeleteVote(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}