using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PriceChangeLogService
    {
        private readonly ApplicationDbContext _context;

        public PriceChangeLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PriceChangeLog>> GetAllAsync()
        {
            return await _context.PriceChangeLogs
                .OrderByDescending(pcl => pcl.ChangeDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PriceChangeLog?> GetByIdAsync(Guid id)
        {
            return await _context.PriceChangeLogs.FindAsync(id);
        }

        public async Task<PriceChangeLog> CreateAsync(PriceChangeLog entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            _context.PriceChangeLogs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.PriceChangeLogs.FindAsync(id);
            if (existing == null) return false;

            _context.PriceChangeLogs.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
