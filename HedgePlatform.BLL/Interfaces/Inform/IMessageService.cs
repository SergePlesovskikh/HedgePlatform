using HedgePlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HedgePlatform.BLL.Interfaces
{
    public interface IMessageService
    {
        IEnumerable<MessageDTO> GetMessages();
        IEnumerable<VoteDTO> GetMessagesAndVotes(int? ResidentId);
        void CreateMessage(MessageDTO message);
        void EditMessage(MessageDTO message);
        void DeleteMessage(int? id);
        void Dispose();
    }
}
