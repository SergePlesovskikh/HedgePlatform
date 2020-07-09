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
    public class VoteOptionController : Controller
    {
        private IVoteOptionService _voteOptionService;
        public VoteOptionController(IVoteOptionService voteOptionService)
        {
            _voteOptionService = voteOptionService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<VoteOptionDTO, VoteOptionViewModel>().ForMember(s => s.Vote, h => h.MapFrom(src => src.Vote));
            cfg.CreateMap<VoteDTO, VoteViewModel>();
            cfg.CreateMap<VoteOptionViewModel, VoteOptionDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<VoteOptionViewModel> Index()
        {
            IEnumerable<VoteOptionDTO> voteOptionDTOs = _voteOptionService.GetVoteOptions();
            var voteOptions = _mapper.Map<IEnumerable<VoteOptionDTO>, List<VoteOptionViewModel>>(voteOptionDTOs);
            return voteOptions;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteOptionViewModel voteOption)
        {
            var voteOptionDTO = _mapper.Map<VoteOptionViewModel, VoteOptionDTO>(voteOption);
            try
            {
                _voteOptionService.CreateVoteOption(voteOptionDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] VoteOptionViewModel voteOption)
        {
            var voteOptionDTO = _mapper.Map<VoteOptionViewModel, VoteOptionDTO>(voteOption);
            try
            {
                _voteOptionService.EditVoteOption(voteOptionDTO);
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
                _voteOptionService.DeleteVoteOption(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _voteOptionService.Dispose();
            base.Dispose(disposing);
        }
    }
}