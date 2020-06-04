using Microsoft.AspNetCore.Mvc;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using HedgePlatform.ViewModel.API;
using HedgePlatform.BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace HedgePlatform.Controllers.API.Message
{
    
    [ApiController]
    [Route("api/mobile/work/[controller]")]
    public class MessageController : Controller
    {
        private IMessageService _messageService;
        public MessageController (IMessageService messageService)
        {
            _messageService = messageService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteDTO, VoteViewModel>()
                .ForMember(s => s.VoteOptions, h => h.MapFrom(src => src.VoteOptions));
                cfg.CreateMap<VoteOptionDTO, VoteOptionViewModel>();
            }).CreateMapper();

        [HttpGet]        
        public IEnumerable<VoteViewModel> Index()
        {
            IEnumerable<VoteDTO> voteDTOs = _messageService.GetMessagesAndVotes((int)HttpContext.Items["ResidentId"]);
            var messages = _mapper.Map<IEnumerable<VoteDTO>, List<VoteViewModel>>(voteDTOs);
            return messages;
        }
        protected override void Dispose(bool disposing)
        {
            _messageService.Dispose();
            base.Dispose(disposing);
        }
    }
}