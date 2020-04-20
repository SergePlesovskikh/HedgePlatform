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
    public class MessageController : ControllerBase
    {
        IMessageService messageService;
        public MessageController(IMessageService service)
        {
            messageService = service;
        }

        [HttpGet]
        public IEnumerable<MessageViewModel> Index()
        {
            IEnumerable<MessageDTO> messageDTOs = messageService.GetMessages();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageDTO, MessageViewModel>()).CreateMapper();
            var messages = mapper.Map<IEnumerable<MessageDTO>, List<MessageViewModel>>(messageDTOs);
            return messages;
        }

        [HttpPost]
        public IActionResult Create([FromBody] MessageViewModel message)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageViewModel, MessageDTO>()).CreateMapper();
            var messageDTO = mapper.Map<MessageViewModel, MessageDTO>(message);
            try
            {
                messageService.CreateMessage(messageDTO);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit([FromBody] MessageViewModel message)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageViewModel, MessageDTO>()).CreateMapper();
            var messageDTO = mapper.Map<MessageViewModel, MessageDTO>(message);
            try
            {
                messageService.EditMessage(messageDTO);
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
                messageService.DeleteMessage(id);
                return Ok("Ok");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}