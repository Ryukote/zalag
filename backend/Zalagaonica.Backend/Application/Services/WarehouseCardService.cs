using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class WarehouseCardService
    {
        private readonly ApplicationDbContext _context;

        public WarehouseCardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WarehouseCard>> GetAllAsync()
        {
            return await _context.WarehouseCards
                .OrderBy(wc => wc.Warehouse)
                .ThenBy(wc => wc.ArticleCode)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<WarehouseCard>> GetByWarehouseAsync(string warehouse)
        {
            return await _context.WarehouseCards
                .Where(wc => wc.Warehouse == warehouse)
                .OrderBy(wc => wc.ArticleCode)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WarehouseCard?> GetByIdAsync(Guid id)
        {
            return await _context.WarehouseCards.FindAsync(id);
        }

        public async Task<WarehouseCard> CreateAsync(WarehouseCard entity)
        {
            entity.Id = Guid.NewGuid();
            entity.LastMovement = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;

            _context.WarehouseCards.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(WarehouseCard entity)
        {
            var existing = await _context.WarehouseCards.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.ArticleCode = entity.ArticleCode;
            existing.ArticleName = entity.ArticleName;
            existing.Warehouse = entity.Warehouse;
            existing.CurrentStock = entity.CurrentStock;
            existing.ReservedStock = entity.ReservedStock;
            existing.UnitOfMeasure = entity.UnitOfMeasure;
            existing.LastMovement = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.WarehouseCards.FindAsync(id);
            if (existing == null) return false;

            _context.WarehouseCards.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
