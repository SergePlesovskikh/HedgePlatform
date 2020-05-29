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

namespace HedgePlatform.BLL.Services
{
    public class VoteService : IVoteService
    {
        IUnitOfWork db { get; set; }

        public VoteService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<VoteService>();

        public VoteDTO GetVote(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var vote = db.Votes.Get(id.Value);
            if (vote == null)
                throw new ValidationException("NOT_FOUND", "");

            return new VoteDTO { Id = vote.Id, Content = vote.Content, DateMessage = vote.DateMessage,
                Header = vote.Header };
        }

        public IEnumerable<VoteDTO> GetVotes()        
        {
            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<Vote, VoteDTO>().ForMember(s => s.VoteOptions, h => h.MapFrom(src => src.VoteOptions));
                cfg.CreateMap<VoteOption, VoteOptionDTO>();
            }).CreateMapper();
            IEnumerable<Vote> votes = db.Votes.GetWithInclude(x => x.Discriminator == "Vote", x => x.VoteOptions);
            return mapper.Map<IEnumerable<Vote>, List<VoteDTO>>(votes);
        }

        public void CreateVote(VoteDTO vote)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteDTO, Vote>()).CreateMapper();
            try
            {
                db.Votes.Create(mapper.Map<VoteDTO, Vote>(vote));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Vote creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }

        public void EditVote(VoteDTO vote)
        {
            if (vote == null)
                throw new ValidationException("No Vote object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteDTO, Vote>()).CreateMapper();
            try
            {
                db.Votes.Update(mapper.Map<VoteDTO, Vote>(vote));
                db.Save();
                _logger.LogInformation("Edit Vote: " + vote.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Vote edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Vote edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteVote(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var vote = db.Votes.Get(id.Value);
            if (vote == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.Votes.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete Vote: " + vote.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Vote delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("Vote delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
