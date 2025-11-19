using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UnitOfMeasureService
    {
        private readonly ApplicationDbContext _context;
        public UnitOfMeasureService(ApplicationDbContext context) => _context = context;

        public async Task<List<UnitOfMeasure>> GetAllAsync() =>
            await _context.UnitsOfMeasure.AsNoTracking().ToListAsync();

        public async Task<UnitOfMeasure?> GetByIdAsync(Guid id) =>
            await _context.UnitsOfMeasure.FindAsync(id);

        public async Task<UnitOfMeasure> CreateAsync(UnitOfMeasure entity)
        {
            entity.Id = Guid.NewGuid();
            _context.UnitsOfMeasure.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(UnitOfMeasure entity)
        {
            var existing = await _context.UnitsOfMeasure.FindAsync(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.UnitsOfMeasure.FindAsync(id);
            if (existing == null) return false;
            _context.UnitsOfMeasure.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
