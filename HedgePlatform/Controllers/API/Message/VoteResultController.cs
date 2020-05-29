using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.API.Message
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class VoteResultController : ControllerBase
    {
        private IVoteResultService _voteResultService;
        public VoteResultController (IVoteResultService voteResultService)
        {
            _voteResultService = voteResultService;
        }

        [HttpGet]
        public FileContentResult Get()
        {            
            return File(_voteResultService.GetVoteStat((int)HttpContext.Items["ResidentId"]), "application/pdf");
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultViewModel, VoteResultDTO>()).CreateMapper();
            var voteOptionDTO = mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                _voteResultService.CreateVoteResult(voteOptionDTO, (int)HttpContext.Items["ResidentId"]);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}