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
    public class VoteService : IVoteService
    {
        private IUnitOfWork _db { get; set; }    
       
        public VoteService(IUnitOfWork uow)
        {
            _db = uow;           
        }
        
        private readonly ILogger _logger = Log.CreateLogger<VoteService>();
        private static IMapper _mapper =  new MapperConfiguration(cfg => {
            cfg.CreateMap<Vote, VoteDTO>().ForMember(s => s.VoteOptions, h => h.MapFrom(src => src.VoteOptions));
            cfg.CreateMap<VoteOption, VoteOptionDTO>();
            cfg.CreateMap<VoteDTO, Vote>();
        }).CreateMapper();

        public VoteDTO GetVote(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var vote = _db.Votes.Get(id.Value);
            if (vote == null)
                throw new ValidationException("NOT_FOUND", "");
            return _mapper.Map<Vote, VoteDTO>(vote); 
        }

        public IEnumerable<VoteDTO> GetVotes()        
        {
            IEnumerable<Vote> votes = _db.Votes.GetWithInclude(x => x.Discriminator == "Vote", x => x.VoteOptions);
            return _mapper.Map<IEnumerable<Vote>, List<VoteDTO>>(votes);
        }

        public bool CheckVoteResident(VoteDTO voteDTO, int ResidentId, IEnumerable<VoteResultDTO> voteResultDTOs)
        {
            return voteResultDTOs.Where(x => x.VoteOption.VoteId == voteDTO.Id).Count() == 0;           
        }

        public void CreateVote(VoteDTO vote)
        {
            try
            {
                _db.Votes.Create(_mapper.Map<VoteDTO, Vote>(vote));
                _db.Save();
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
            try
            {
                _db.Votes.Update(_mapper.Map<VoteDTO, Vote>(vote));
                _db.Save();
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

            var vote = _db.Votes.Get(id.Value);
            if (vote == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.Votes.Delete(id.Value);
                _db.Save();
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
            _db.Dispose();
        }
    }
}
