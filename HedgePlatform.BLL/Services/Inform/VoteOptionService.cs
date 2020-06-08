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
        private IUnitOfWork _db { get; set; }

        public VoteOptionService(IUnitOfWork uow)
        {
            _db = uow;
        }

        private readonly ILogger _logger = Log.CreateLogger<VoteOptionService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<VoteOption, VoteOptionDTO>().ForMember(s => s.Vote, h => h.MapFrom(src => src.Vote));
            cfg.CreateMap<Vote, VoteDTO>();
            cfg.CreateMap<VoteOptionDTO, VoteOption>();
        }).CreateMapper();

        public IEnumerable<VoteOptionDTO> GetVoteOptions()
        {
            var voteOptions = _db.VoteOptions.GetWithInclude(x => x.Vote);
            return _mapper.Map<IEnumerable<VoteOption>, List<VoteOptionDTO>>(voteOptions);
        }

        public void CreateVoteOption(VoteOptionDTO voteOption)
        {
            try
            {
                _db.VoteOptions.Create(_mapper.Map<VoteOptionDTO, VoteOption>(voteOption));
                _db.Save();
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
          
            try
            {
                _db.VoteOptions.Update(_mapper.Map<VoteOptionDTO, VoteOption>(voteOption));
                _db.Save();
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

            var voteOption = _db.VoteOptions.Get(id.Value);
            if (voteOption == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.VoteOptions.Delete(id.Value);
                _db.Save();
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
            _db.Dispose();
        }
    }
}
