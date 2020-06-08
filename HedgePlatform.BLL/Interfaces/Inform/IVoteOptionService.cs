using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IVoteOptionService
    {      
        IEnumerable<VoteOptionDTO> GetVoteOptions();
        void CreateVoteOption(VoteOptionDTO voteOption);
        void EditVoteOption(VoteOptionDTO voteOption);
        void DeleteVoteOption(int? id);
        void Dispose();
    }
}
