using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class LabelService
    {
        private readonly ApplicationDbContext _context;

        public LabelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Label>> GetAllAsync()
        {
            return await _context.Labels
                .OrderByDescending(l => l.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Label?> GetByIdAsync(Guid id)
        {
            return await _context.Labels.FindAsync(id);
        }

        public async Task<Label> CreateAsync(Label entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            _context.Labels.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Label entity)
        {
            var existing = await _context.Labels.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Labels.FindAsync(id);
            if (existing == null) return false;

            _context.Labels.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
