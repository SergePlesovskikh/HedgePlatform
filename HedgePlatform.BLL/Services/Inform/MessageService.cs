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
        private IUnitOfWork _db { get; set; }
        private IVoteService _voteService;
        private IResidentService _residentService;
        private IVoteResultService _voteResultService;
        private readonly ILogger _logger;

        public MessageService(IUnitOfWork uow, IVoteService  voteService, IResidentService residentService, 
            IVoteResultService voteResultService, ILogger<MessageService> logger)
        {
            _db = uow;
            _voteService = voteService;
            _residentService = residentService;
            _voteResultService = voteResultService;
            _logger = logger;
        }
        
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<Message, MessageDTO>();
            cfg.CreateMap<MessageDTO, Message>();
            cfg.CreateMap<MessageDTO, VoteDTO>();
        }).CreateMapper();

        public IEnumerable<MessageDTO> GetMessages() => _mapper.Map<IEnumerable<Message>, List<MessageDTO>>(_db.Messages.Find(x => x.Discriminator == "Message"));        
       
        public IEnumerable<VoteDTO> GetMessagesAndVotes(int? ResidentId)
        {
            if (ResidentId == null)
                throw new ValidationException("NULL", "RESIDENT_ID");

            List<VoteDTO> voteDTOs = new List<VoteDTO> { };
            if (_residentService.CheckChairman(ResidentId.Value))
            {
                voteDTOs = _voteService.GetVotes().ToList();
                List<VoteDTO> actual_voteDTOs = new List<VoteDTO> { };
                IEnumerable<VoteResultDTO> voteResultDTOs = _voteResultService.GetVoteResultsByResident(ResidentId);
                foreach (var voteDTO in voteDTOs)
                {                    
                   if(_voteService.CheckVoteResident(voteDTO, ResidentId.Value, voteResultDTOs))
                        actual_voteDTOs.Add(voteDTO);
                }
                voteDTOs = actual_voteDTOs;
            }               
           
            IEnumerable<VoteDTO> messages = _mapper.Map<IEnumerable<MessageDTO>, IEnumerable<VoteDTO>>(GetMessages());
            return voteDTOs.Union(messages);
        }

        public void CreateMessage(MessageDTO message)
        {
            try
            {
                _db.Messages.Create(_mapper.Map<MessageDTO, Message>(message));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Message creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Message creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void EditMessage(MessageDTO message)
        {
            if (message == null)
                throw new ValidationException("NO_OBJECT", "");

            try
            {
                _db.Messages.Update(_mapper.Map<MessageDTO, Message>(message));
                _db.Save();
                _logger.LogInformation("Edit Message: " + message.Id);
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Message edit Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Message edit error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteMessage(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "MESSAGE_ID");

            var message = _db.Messages.Get(id.Value);
            if (message == null)
                throw new ValidationException("NOT_FOUND", "");

            try
            {
                _db.Messages.Delete(id.Value);
                _db.Save();
                _logger.LogInformation($"Delete Message: {message.Id}");
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"Message delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }
            catch (Exception ex)
            {
                _logger.LogError("Message delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose() => _db.Dispose();        
    }
}
