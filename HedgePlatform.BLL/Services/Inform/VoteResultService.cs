using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using HedgePlatform.DAL.Entities;
using HedgePlatform.BLL.Infr;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.BLL.Services
{
    public class VoteResultService : IVoteResultService
    {
        IUnitOfWork db { get; set; }

        public VoteResultService(IUnitOfWork uow)
        {
            db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<VoteResultService>();

        public VoteResultDTO GetVoteResult(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");
            var voteResult = db.VoteResults.Get(id.Value);
            if (voteResult == null)
                throw new ValidationException("NOT_FOUND", "");

            Resident resident = db.Residents.Get(voteResult.ResidentId);
            VoteOption voteOption = db.VoteOptions.Get(voteResult.VoteOptionId);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Resident, ResidentDTO>();
                cfg.CreateMap<VoteOption, VoteOptionDTO>();
            }
               ).CreateMapper();

            return new VoteResultDTO
            {
                Id = voteResult.Id,
                ResidentId = voteResult.ResidentId,
                Resident = mapper.Map<Resident, ResidentDTO>(resident),
                DateVote = voteResult.DateVote,
                VoteOptionId = voteResult.VoteOptionId,
                VoteOption = mapper.Map<VoteOption, VoteOptionDTO>(voteOption),

            };
        }

        public IEnumerable<VoteResultDTO> GetVoteResults()
        {
            var mapper = new MapperConfiguration(cfg => {
                cfg.CreateMap<VoteResult, VoteResultDTO>().ForMember(s => s.Resident, h => h.MapFrom(src => src.Resident))
                .ForMember(s => s.VoteOption, h => h.MapFrom(src => src.VoteOption));
                cfg.CreateMap<Resident, ResidentDTO>();
                cfg.CreateMap<VoteOption, VoteOptionDTO>();
            }).CreateMapper();
            var voteResults = db.VoteResults.GetWithInclude(x => x.Resident);
            return mapper.Map<IEnumerable<VoteResult>, List<VoteResultDTO>>(voteResults);
        }

        public void CreateVoteResult(VoteResultDTO voteResult)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultDTO, VoteResult>()).CreateMapper();
            try
            {
                db.VoteResults.Create(mapper.Map<VoteResultDTO, VoteResult>(voteResult));
                db.Save();
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("Database error exception: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteResult creating error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }

        }
        public void EditVoteResult(VoteResultDTO voteResult)
        {
            if (voteResult == null)
                throw new ValidationException("No voteResult object", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VoteResultDTO, VoteResult>()).CreateMapper();
            try
            {
                db.VoteResults.Update(mapper.Map<VoteResultDTO, VoteResult>(voteResult));
                db.Save();
                _logger.LogInformation("Edit voteResult: " + voteResult.Id);
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError("voteResult edit db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteResult edit error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteVoteResult(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "");

            var voteResult = db.VoteResults.Get(id.Value);
            if (voteResult == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                db.VoteResults.Delete(id.Value);
                db.Save();
                _logger.LogInformation("Delete voteResult: " + voteResult.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("voteResult delete db error: " + ex.InnerException.Message);
                throw new ValidationException("DB_ERROR", "");
            }

            catch (Exception ex)
            {
                _logger.LogError("voteResult delete error: " + ex.Message);
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}
