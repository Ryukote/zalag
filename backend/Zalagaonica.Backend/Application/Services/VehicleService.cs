using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class VehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.AsNoTracking().ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<Vehicle> CreateAsync(Vehicle entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Vehicles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Vehicle entity)
        {
            var existing = await _context.Vehicles.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Vehicles.FindAsync(id);
            if (existing == null) return false;

            _context.Vehicles.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
