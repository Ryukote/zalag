using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class VehicleStatusService
    {
        private readonly ApplicationDbContext _context;

        public VehicleStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleStatus>> GetAllAsync()
        {
            return await _context.VehicleStatuses.AsNoTracking().ToListAsync();
        }

        public async Task<VehicleStatus?> GetByIdAsync(Guid id)
        {
            return await _context.VehicleStatuses.FindAsync(id);
        }

        public async Task<VehicleStatus> CreateAsync(VehicleStatus entity)
        {
            entity.Id = Guid.NewGuid();
            _context.VehicleStatuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(VehicleStatus entity)
        {
            var existing = await _context.VehicleStatuses.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.VehicleStatuses.FindAsync(id);
            if (existing == null) return false;

            _context.VehicleStatuses.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
