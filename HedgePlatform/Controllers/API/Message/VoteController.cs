using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.API.Message
{
    [Route("api/inform/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private IVoteResultService _voteResultService;
        public VoteController (IVoteResultService voteResultService)
        {
            _voteResultService = voteResultService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultViewModel, VoteResultDTO>()).CreateMapper();
            var voteOptionDTO = mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                _voteResultService.CreateVoteResult(voteOptionDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}