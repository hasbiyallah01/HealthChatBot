using HealthChatBox.Core.Application.Interfaces.Repositories;
using HealthChatBox.Core.Domain.Entities;
using HealthChatBox.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthChatBox.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly KonsumeContext _context;
        public ChatRepository(KonsumeContext context)
        {
            _context = context;
        }

        public async Task<ChatEntry> AddAsync(ChatEntry user)
        {
            await _context.Set<ChatEntry>()
                .AddAsync(user);
            return await _context.ChatEntries.OrderByDescending(user => user.DateCreated).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.ChatEntries.AnyAsync(x => x.Id == id);
        }

        public ChatEntry Update(ChatEntry entity)
        {
            _context.ChatEntries.Update(entity);
            return entity;
        }

        public async Task<ICollection<ChatEntry>> GetAllAsync()
        {
            var answer = await _context.Set<ChatEntry>()
                            .ToListAsync();
            return answer;
        }

        public async Task<ChatEntry> GetAsync(int id)
        {
            var answer = await _context.Set<ChatEntry>()
                        .Where(a => !a.IsDeleted && a.Id == id)
                        .SingleOrDefaultAsync();
            return answer;
        }

        public async Task<ChatEntry> GetAsync(Expression<Func<ChatEntry, bool>> exp)
        {
            var answer = await _context.Set<ChatEntry>()
                        .Where(a => !a.IsDeleted)
                        .SingleOrDefaultAsync(exp);
            return answer;
        }

        public void Remove(ChatEntry answer)
        {
            answer.IsDeleted = true;
            _context.Set<ChatEntry>()
                .Update(answer);
            _context.SaveChanges();
        }
    }
}
