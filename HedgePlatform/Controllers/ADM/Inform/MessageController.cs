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
    public class MessageController : Controller
    {
        private IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<MessageDTO, MessageViewModel>();
            cfg.CreateMap<MessageViewModel, MessageDTO>();
        }).CreateMapper();

        [HttpGet]
        public IEnumerable<MessageViewModel> Index()
        {
            IEnumerable<MessageDTO> messageDTOs = _messageService.GetMessages();
            var messages = _mapper.Map<IEnumerable<MessageDTO>, List<MessageViewModel>>(messageDTOs);
            return messages;
        }

        [HttpPost]
        public IActionResult Create([FromBody] MessageViewModel message)
        {
            var messageDTO = _mapper.Map<MessageViewModel, MessageDTO>(message);
            try
            {
                _messageService.CreateMessage(messageDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] MessageViewModel message)
        {
            var messageDTO = _mapper.Map<MessageViewModel, MessageDTO>(message);
            try
            {
                _messageService.EditMessage(messageDTO);
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
                _messageService.DeleteMessage(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"{ex.Message}:{ex.Property}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            _messageService.Dispose();
            base.Dispose(disposing);
        }
    }
}