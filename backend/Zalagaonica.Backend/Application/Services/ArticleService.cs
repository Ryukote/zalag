using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _context.Articles.AsNoTracking().ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(Guid id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Article> CreateAsync(Article entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Articles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Article entity)
        {
            var existing = await _context.Articles.FindAsync(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Articles.FindAsync(id);
            if (existing == null) return false;
            _context.Articles.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

