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
    public class VoteOptionController : ControllerBase
    {
        IVoteOptionService voteOptionService;
        public VoteOptionController(IVoteOptionService service)
        {
            voteOptionService = service;
        }
        [HttpGet]
        public IEnumerable<VoteOptionViewModel> Index()
        {
            IEnumerable<VoteOptionDTO> voteOptionDTOs = voteOptionService.GetVoteOptions();

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteOptionDTO, VoteOptionViewModel>().ForMember(s => s.Vote, h => h.MapFrom(src => src.Vote));
                cfg.CreateMap<VoteDTO, VoteViewModel>();
            }).CreateMapper();

            var voteOptions = mapper.Map<IEnumerable<VoteOptionDTO>, List<VoteOptionViewModel>>(voteOptionDTOs);
            return voteOptions;
        }

        [HttpPost]
        public IActionResult Create([FromBody] VoteOptionViewModel voteOption)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteOptionViewModel, VoteOptionDTO>()).CreateMapper();
            var voteOptionDTO = mapper.Map<VoteOptionViewModel, VoteOptionDTO>(voteOption);
            try
            {
                voteOptionService.CreateVoteOption(voteOptionDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] VoteOptionViewModel voteOption)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteOptionViewModel, VoteOptionDTO>()).CreateMapper();
            var voteOptionDTO = mapper.Map<VoteOptionViewModel, VoteOptionDTO>(voteOption);
            try
            {
                voteOptionService.EditVoteOption(voteOptionDTO);
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
                voteOptionService.DeleteVoteOption(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}