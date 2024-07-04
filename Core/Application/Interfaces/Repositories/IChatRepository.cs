using HealthChatBox.Core.Domain.Entities;
using System.Linq.Expressions;

namespace HealthChatBox.Core.Application.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<ChatEntry> AddAsync(ChatEntry entry);
        Task<bool> ExistAsync(int id);
        Task<ChatEntry> GetAsync(int id);
        Task<ChatEntry> GetAsync(Expression<Func<ChatEntry, bool>> exp);
        Task<ICollection<ChatEntry>> GetAllAsync();
        void Remove(ChatEntry entry);
        ChatEntry Update(ChatEntry entry);
    }
}
