using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class InventoryCountService
    {
        private readonly ApplicationDbContext _context;

        public InventoryCountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryCount>> GetAllAsync()
        {
            return await _context.InventoryCounts
                .OrderByDescending(ic => ic.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<InventoryCount>> GetByInventoryNumberAsync(string inventoryNumber)
        {
            return await _context.InventoryCounts
                .Where(ic => ic.InventoryNumber == inventoryNumber)
                .OrderBy(ic => ic.ArticleCode)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InventoryCount?> GetByIdAsync(Guid id)
        {
            return await _context.InventoryCounts.FindAsync(id);
        }

        public async Task<InventoryCount> CreateAsync(InventoryCount entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            _context.InventoryCounts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(InventoryCount entity)
        {
            var existing = await _context.InventoryCounts.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.InventoryNumber = entity.InventoryNumber;
            existing.Date = entity.Date;
            existing.ArticleCode = entity.ArticleCode;
            existing.ArticleName = entity.ArticleName;
            existing.BookQuantity = entity.BookQuantity;
            existing.PhysicalQuantity = entity.PhysicalQuantity;
            existing.Warehouse = entity.Warehouse;
            existing.Status = entity.Status;
            existing.Notes = entity.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.InventoryCounts.FindAsync(id);
            if (existing == null) return false;

            _context.InventoryCounts.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(Guid id)
        {
            var existing = await _context.InventoryCounts.FindAsync(id);
            if (existing == null) return false;

            existing.Status = "approved";
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
