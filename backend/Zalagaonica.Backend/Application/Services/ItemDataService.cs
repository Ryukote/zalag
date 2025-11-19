using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ItemDataService
    {
        private readonly ApplicationDbContext _context;

        public ItemDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemData>> GetAllAsync() =>
            await _context.ItemData.AsNoTracking().ToListAsync();

        public async Task<ItemData?> GetByIdAsync(Guid id) =>
            await _context.ItemData.FindAsync(id);

        public async Task<ItemData> CreateAsync(ItemData entity)
        {
            entity.Id = Guid.NewGuid();
            _context.ItemData.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(ItemData entity)
        {
            var existing = await _context.ItemData.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.ItemData.FindAsync(id);
            if (existing == null) return false;

            _context.ItemData.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
