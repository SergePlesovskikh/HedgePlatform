using Microsoft.AspNetCore.Mvc;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Infr;
using AutoMapper;

namespace HedgePlatform.Controllers.API
{
    [Route("api/mobile/work/[controller]")]
    [ApiController]
    public class VoteResultController : Controller
    {
        private IVoteResultService _voteResultService;
        public VoteResultController (IVoteResultService voteResultService)
        {
            _voteResultService = voteResultService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultViewModel, VoteResultDTO>()).CreateMapper();

        [HttpGet]
        public FileContentResult Get()
        {            
            return File(_voteResultService.GetVoteStat((int)HttpContext.Items["ResidentId"]), "application/pdf");
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var voteOptionDTO = _mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                _voteResultService.CreateVoteResult(voteOptionDTO, (int)HttpContext.Items["ResidentId"]);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _voteResultService.Dispose();
            base.Dispose(disposing);
        }
    }
}