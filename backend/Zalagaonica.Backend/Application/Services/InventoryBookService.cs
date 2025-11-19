using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class InventoryBookService
    {
        private readonly ApplicationDbContext _context;

        public InventoryBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryBook>> GetAllAsync()
        {
            return await _context.InventoryBooks
                .OrderByDescending(ib => ib.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<InventoryBook>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.InventoryBooks
                .Where(ib => ib.Date >= startDate && ib.Date <= endDate)
                .OrderByDescending(ib => ib.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InventoryBook?> GetByIdAsync(Guid id)
        {
            return await _context.InventoryBooks.FindAsync(id);
        }

        public async Task<InventoryBook> CreateAsync(InventoryBook entity)
        {
            entity.Id = Guid.NewGuid();
            entity.TotalSale = entity.QuantitySold * entity.SalePrice;
            entity.CreatedAt = DateTime.UtcNow;

            _context.InventoryBooks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.InventoryBooks.FindAsync(id);
            if (existing == null) return false;

            _context.InventoryBooks.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
