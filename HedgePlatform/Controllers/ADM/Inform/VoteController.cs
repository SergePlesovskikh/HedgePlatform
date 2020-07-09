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
    public class VoteController : Controller
    {
        private IVoteService _voteService;
        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<VoteDTO, VoteViewModel>();
            cfg.CreateMap<VoteViewModel, VoteDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<VoteViewModel> Index()
        {
            IEnumerable<VoteDTO> voteDTOs = _voteService.GetVotes();
            var counterStats = _mapper.Map<IEnumerable<VoteDTO>, List<VoteViewModel>>(voteDTOs);
            return counterStats;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteViewModel vote)
        {
            var voteDTO = _mapper.Map<VoteViewModel, VoteDTO>(vote);
            try
            {
                _voteService.CreateVote(voteDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] VoteViewModel vote)
        {
            var voteDTO = _mapper.Map<VoteViewModel, VoteDTO>(vote);
            try
            {
                _voteService.EditVote(voteDTO);
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
                _voteService.DeleteVote(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _voteService.Dispose();
            base.Dispose(disposing);
        }
    }
}