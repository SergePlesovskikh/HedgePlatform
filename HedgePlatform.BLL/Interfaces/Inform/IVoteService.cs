using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IVoteService
    {
        VoteDTO GetVote(int? id);
        IEnumerable<VoteDTO> GetVotes();
        bool CheckVoteResident(VoteDTO voteDTO, int ResidentId, IEnumerable<VoteResultDTO> voteResultDTOs);
        void CreateVote(VoteDTO vote);
        void EditVote(VoteDTO vote);
        void DeleteVote(int? id);
        void Dispose();
    }
}
