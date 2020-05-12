using HedgePlatform.BLL.DTO;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Linq;

namespace HedgePlatform.BLL.Services
{
    public class MessageService : IMessageService
    {
        IUnitOfWork db { get; set; }
        private IVoteService _voteService;
        private IResidentService _residentService;

        public MessageService(IUnitOfWork uow, IVoteService  voteService, IResidentService residentService)
        {
            db = uow;
            _voteService = voteService;
            _residentService = residentService;
        }

        private readonly ILogger _logger = Log.CreateLogger<MessageService>();

        public MessageDTO GetMessage(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var message = db.Messages.Get(id.Value);
            if (message == null)
                throw new ValidationException("NOT_FOUND", "");

            return new MessageDTO { Id = message.Id, Content = message.Content, DateMessage = message.DateMessage, Header = message.Header };
        }

        public IEnumerable<MessageDTO> GetMessages()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Message, MessageDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Message>, List<MessageDTO>>(db.Messages.GetAll());
        }

        public IEnumerable<VoteDTO> GetMessagesAndVotes(int? ResidentId)
        {
            if (ResidentId == null)
                throw new ValidationException("No Resident Id", "");
            ResidentDTO residentDTO = _residentService.GetResident(ResidentId);
            IEnumerable<VoteDTO> voteDTOs = Enumerable.Empty<VoteDTO>();
            if (residentDTO.Chairman.Value)
                voteDTOs = _voteService.GetVotes();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageDTO, VoteDTO>()).CreateMapper();
            IEnumerable<VoteDTO> messages = mapper.Map<IEnumerable<MessageDTO>, IEnumerable<VoteDTO>>(GetMessages());
            return voteDTOs.Union(messages);
        }

        public void CreateMessage(MessageDTO message)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageDTO, Message>()).CreateMapper();
            try
            {
                db.Messages.Create(mapper.Map<MessageDTO, Message>(message));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Message creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditMessage(MessageDTO message)
        {
            if (message == null)
                throw new ValidationException("No Message object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MessageDTO, Message>()).CreateMapper();
            try
            {
                db.Messages.Update(mapper.Map<MessageDTO, Message>(message));
                db.Save();
                _logger.LogInformation("Edit Message: " + message.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Message edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Message edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteMessage(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var message = db.Messages.Get(id.Value);
            if (message == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Messages.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete Message: " + message.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Message delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Message delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
