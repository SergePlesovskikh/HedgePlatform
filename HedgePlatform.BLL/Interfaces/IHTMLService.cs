using HedgePlatform.BLL.DTO;
using System.Collections.Generic;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IHTMLService
    {
        string GenerateHTMLRequest(ResidentDTO resident);
        string GenerateVoteStat(IEnumerable<VoteResultDTO> voteResultDTOs);
    }
}
