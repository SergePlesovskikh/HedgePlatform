using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.API.Message
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private IVoteOptionService _voteOptionService;
        public VoteController (IVoteOptionService voteOptionService)
        {
            _voteOptionService = voteOptionService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteOptionViewModel, VoteOptionDTO>()).CreateMapper();
            var voteOptionDTO = mapper.Map<VoteOptionViewModel, VoteOptionDTO>(voteOption);
            try
            {
                _voteOptionService.CreateVoteOption(voteOptionDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}