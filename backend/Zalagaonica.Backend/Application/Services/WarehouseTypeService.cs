using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class WarehouseTypeService
    {
        private readonly ApplicationDbContext _context;
        public WarehouseTypeService(ApplicationDbContext context) => _context = context;

        public async Task<List<WarehouseType>> GetAllAsync() =>
            await _context.WarehouseTypes.AsNoTracking().ToListAsync();

        public async Task<WarehouseType?> GetByIdAsync(Guid id) =>
            await _context.WarehouseTypes.FindAsync(id);

        public async Task<WarehouseType> CreateAsync(WarehouseType entity)
        {
            entity.Id = Guid.NewGuid();
            _context.WarehouseTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(WarehouseType entity)
        {
            var existing = await _context.WarehouseTypes.FindAsync(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.WarehouseTypes.FindAsync(id);
            if (existing == null) return false;
            _context.WarehouseTypes.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
