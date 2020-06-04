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
    public class VoteResultController : Controller
    {
        private IVoteResultService _voteResultService;
        public VoteResultController(IVoteResultService voteResultService)
        {
            _voteResultService = voteResultService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<VoteResultDTO, VoteResultViewModel>().ForMember(s => s.VoteOption, h => h.MapFrom(src => src.VoteOption))
            .ForMember(s => s.Resident, h => h.MapFrom(src => src.Resident));
            cfg.CreateMap<ResidentDTO, ResidentViewModel>();
            cfg.CreateMap<VoteOptionDTO, VoteOptionViewModel>();
            cfg.CreateMap<VoteResultViewModel, VoteResultDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<VoteResultViewModel> Index()
        {
            IEnumerable<VoteResultDTO> voteResultDTOs = _voteResultService.GetVoteResults();
            var voteResults = _mapper.Map<IEnumerable<VoteResultDTO>, List<VoteResultViewModel>>(voteResultDTOs);
            return voteResults;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteResultViewModel voteResult)
        {
            var voteResultDTO = _mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                _voteResultService.CreateVoteResult(voteResultDTO);
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
            var voteResultDTO = _mapper.Map<VoteResultViewModel, VoteResultDTO>(voteResult);
            try
            {
                _voteResultService.EditVoteResult(voteResultDTO);
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
                _voteResultService.DeleteVoteResult(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            _voteResultService.Dispose();
            base.Dispose(disposing);
        }
    }
}