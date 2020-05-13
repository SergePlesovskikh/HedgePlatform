using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IHTMLService
    {
        string GenerateHTMLRequest(ResidentDTO resident);
        string GenerateVoteStat(IEnumerable<VoteResultDTO> voteResultDTOs);
    }
}
