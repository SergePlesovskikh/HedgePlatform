using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IVoteResultService
    {     
        IEnumerable<VoteResultDTO> GetVoteResults();
        IEnumerable<VoteResultDTO> GetVoteResultsByResident(int? ResidentId);
        byte[] GetVoteStat(int? ResidentId);
        void CreateVoteResult(VoteResultDTO voteResult);
        void CreateVoteResult(VoteResultDTO voteResult, int? ResidentId);
        void EditVoteResult(VoteResultDTO voteResult);
        void DeleteVoteResult(int? id);
        void Dispose();
    }
}
