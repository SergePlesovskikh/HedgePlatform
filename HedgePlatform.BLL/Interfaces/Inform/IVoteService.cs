using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IVoteService
    {
        VoteResultDTO GetVoteResult(int? id);
        IEnumerable<VoteResultDTO> GetVoteResults();
        void CreateVoteResult(VoteResultDTO voteResult);
        void EditVoteResult(VoteResultDTO voteResult);
        void DeleteVoteResult(int? id);
        void Dispose();
    }
}
