using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Controllers.API.Message
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IMessageService _messageService;
        public MessageController (IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public IEnumerable<VoteViewModel> Index()
        {
            IEnumerable<VoteDTO> voteDTOs = _messageService.GetMessagesAndVotes((int)HttpContext.Items["ResidentId"]);

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteDTO, VoteViewModel>()
                .ForMember(s => s.VoteOptions, h => h.MapFrom(src => src.VoteOptions));
                cfg.CreateMap<VoteOptionDTO, VoteViewModel>();
            }).CreateMapper();

            var messages = mapper.Map<IEnumerable<VoteDTO>, List<VoteViewModel>>(voteDTOs);
            return messages;
        }

    }
}