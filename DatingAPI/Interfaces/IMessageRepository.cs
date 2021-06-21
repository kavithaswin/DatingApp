using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Helpers;

namespace DatingAPI.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);

        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);

        Task<bool> SaveAllAsync();
    }
}