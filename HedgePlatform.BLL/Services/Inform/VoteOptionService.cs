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
    public class VoteOptionService : IVoteOptionService
    {
        IUnitOfWork db { get; set; }

        public VoteOptionService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<VoteOptionService>();

        public VoteOptionDTO GetVoteOption(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var voteOption = db.VoteOptions.Get(id.Value);
            if (voteOption == null)
                throw new ValidationException("NOT_FOUND", "");

            Vote flat = db.Votes.Get(voteOption.VoteId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Vote, VoteDTO>()).CreateMapper();

            return new VoteOptionDTO
            {
                Id = voteOption.Id,
                VoteId = voteOption.VoteId,
                Vote = mapper.Map<Vote, VoteDTO>(flat),
                Description = voteOption.Description
            };
        }

        public IEnumerable<VoteOptionDTO> GetVoteOptions()
        {

            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteOption, VoteOptionDTO>().ForMember(s => s.Vote, h => h.MapFrom(src => src.Vote));
                cfg.CreateMap<Vote, VoteDTO>();
            }).CreateMapper();
            var voteOptions = db.VoteOptions.GetWithInclude(x => x.Vote);
            return mapper.Map<IEnumerable<VoteOption>, List<VoteOptionDTO>>(voteOptions);
        }

        public void CreateVoteOption(VoteOptionDTO voteOption)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteOptionDTO, VoteOption>()).CreateMapper();
            try
            {
                db.VoteOptions.Create(mapper.Map<VoteOptionDTO, VoteOption>(voteOption));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteOption creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditVoteOption(VoteOptionDTO voteOption)
        {
            if (voteOption == null)
                throw new ValidationException("No voteOption object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteOptionDTO, VoteOption>()).CreateMapper();
            try
            {
                db.VoteOptions.Update(mapper.Map<VoteOptionDTO, VoteOption>(voteOption));
                db.Save();
                _logger.LogInformation("Edit voteOption: " + voteOption.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("voteOption edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteOption edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteVoteOption(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var voteOption = db.VoteOptions.Get(id.Value);
            if (voteOption == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.VoteOptions.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete voteOption: " + voteOption.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("voteOption delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteOption delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}
