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
        private IUnitOfWork _db { get; set; }
        private IResidentService _residentService;
        private IHTMLService _HTMLService;
        private IPDFService _PDFService;
        private IVoteService _voteService;

        public VoteResultService(IUnitOfWork uow, IResidentService residentService, 
            IVoteService voteService, IPDFService PDFService, IHTMLService HTMLService)
        {
            _db = uow;
            _residentService = residentService;
            _voteService = voteService;
            _HTMLService = HTMLService;
            _PDFService = PDFService;            
        }

        private readonly ILogger _logger = Log.CreateLogger<VoteResultService>();
        private static IMapper _mapper = new MapperConfiguration(cfg => {
            cfg.CreateMap<VoteResult, VoteResultDTO>().ForMember(s => s.Resident, h => h.MapFrom(src => src.Resident))
            .ForMember(s => s.VoteOption, h => h.MapFrom(src => src.VoteOption));
            cfg.CreateMap<Resident, ResidentDTO>();
            cfg.CreateMap<VoteOption, VoteOptionDTO>();
            cfg.CreateMap<VoteResultDTO, VoteResult>();
        }).CreateMapper();
               
        public IEnumerable<VoteResultDTO> GetVoteResults()
        {
            var voteResults = _db.VoteResults.GetWithInclude(x => x.Resident);
            return _mapper.Map<IEnumerable<VoteResult>, List<VoteResultDTO>>(voteResults);
        }

        public IEnumerable<VoteResultDTO> GetVoteResultsByResident(int? ResidentId)
        {
            var voteResults = _db.VoteResults.GetWithInclude(x=>x.ResidentId==ResidentId, x=>x.VoteOption);
            return _mapper.Map<IEnumerable<VoteResult>, List<VoteResultDTO>>(voteResults);
        }

        public byte[] GetVoteStat(int? ResidentId)
        {
            ResidentDTO resident = _residentService.GetResident(ResidentId);
            IEnumerable <VoteResult> voteResults = _db.VoteResults.GetWithInclude(p => p.ResidentId == ResidentId, x => x.VoteOption);
            IEnumerable<VoteResultDTO> voteDTOs = _mapper.Map<IEnumerable<VoteResult>, List<VoteResultDTO>>(voteResults);

            foreach (var voteResult in voteDTOs)
            {
                voteResult.VoteOption.Vote = _voteService.GetVote(voteResult.VoteOption.VoteId);
            }

            string html = _HTMLService.GenerateVoteStat(voteDTOs);
            return _PDFService.PdfConvert(html);
        }

        public void CreateVoteResult(VoteResultDTO voteResult)
        {
            try
            {
                _db.VoteResults.Create(_mapper.Map<VoteResultDTO, VoteResult>(voteResult));
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"voteResult creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"voteResult creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void CreateVoteResult(VoteResultDTO voteResult, int? ResidentId)
        {
            if (ResidentId == null)
                throw new ValidationException("NULL", "RESIDENT_ID");

            if (!_residentService.CheckChairman(ResidentId.Value))
                throw new ValidationException("NO_PERMISSION", "");

            if (!CheckVoteResult(voteResult, ResidentId.Value))
                throw new ValidationException("ALREADY_VOTE", "");
           
            try
            {
                VoteResult new_voteResult = _mapper.Map<VoteResultDTO, VoteResult>(voteResult);
                new_voteResult.ResidentId = ResidentId.Value;
                new_voteResult.DateVote = DateTime.Now;
                _db.VoteResults.Create(new_voteResult);
                _db.Save();
            }

            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"voteResult creating Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"voteResult creating error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }

        public void EditVoteResult(VoteResultDTO voteResult)
        {
            if (voteResult == null)
                throw new ValidationException("NO_OBJECT", "");
           
            try
            {
                _db.VoteResults.Update(_mapper.Map<VoteResultDTO, VoteResult>(voteResult));
                _db.Save();
                _logger.LogInformation("Edit voteResult: " + voteResult.Id);
            }


            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"voteResult editing Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"voteResult editing error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void DeleteVoteResult(int? id)
        {
            if (id == null)
                throw new ValidationException("NULL", "VOTE_RESULT_ID");

            var voteResult = _db.VoteResults.Get(id.Value);
            if (voteResult == null)
                throw new ValidationException("NOT_FOUND", "");
            try
            {
                _db.VoteResults.Delete(id.Value);
                _db.Save();
                _logger.LogInformation("Delete voteResult: " + voteResult.Id);
            }
            catch (DbUpdateException ex)
            {
                DBValidator.SetException(ex);
                _logger.LogError($"voteResult delete Database error exception: {DBValidator.GetErrMessage()}. Property: {DBValidator.GetErrProperty()}");
                throw new ValidationException("DB_ERROR", DBValidator.GetErrProperty());
            }

            catch (Exception ex)
            {
                _logger.LogError($"voteResult delete error: {ex.Message}");
                throw new ValidationException("UNKNOWN_ERROR", "");
            }
        }
        public void Dispose() => _db.Dispose();
  

        private bool CheckVoteResult(VoteResultDTO voteResult, int ResidentId) =>
         _db.VoteResults.FindFirst(x => x.ResidentId == ResidentId && x.VoteOptionId == voteResult.VoteOptionId) == null;             
        
    }
}
