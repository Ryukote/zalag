using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class DailyClosingService
    {
        private readonly ApplicationDbContext _context;

        public DailyClosingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DailyClosing>> GetAllAsync()
        {
            return await _context.DailyClosings
                .OrderByDescending(dc => dc.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DailyClosing?> GetByDateAsync(DateTime date)
        {
            return await _context.DailyClosings
                .FirstOrDefaultAsync(dc => dc.Date.Date == date.Date);
        }

        public async Task<DailyClosing?> GetByIdAsync(Guid id)
        {
            return await _context.DailyClosings.FindAsync(id);
        }

        public async Task<DailyClosing> CreateAsync(DailyClosing entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            _context.DailyClosings.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(DailyClosing entity)
        {
            var existing = await _context.DailyClosings.FindAsync(entity.Id);
            if (existing == null) return false;

            if (existing.IsClosed)
            {
                // Cannot update a closed day
                return false;
            }

            existing.Date = entity.Date;
            existing.CashierName = entity.CashierName;
            existing.StartingCash = entity.StartingCash;
            existing.TotalSales = entity.TotalSales;
            existing.TotalExpenses = entity.TotalExpenses;
            existing.CashInRegister = entity.CashInRegister;
            existing.Notes = entity.Notes;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseDay(Guid id)
        {
            var existing = await _context.DailyClosings.FindAsync(id);
            if (existing == null) return false;

            if (existing.IsClosed)
            {
                // Already closed
                return false;
            }

            existing.IsClosed = true;
            existing.ClosedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.DailyClosings.FindAsync(id);
            if (existing == null) return false;

            if (existing.IsClosed)
            {
                // Cannot delete a closed day
                return false;
            }

            _context.DailyClosings.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
