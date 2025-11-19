using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ReservationStatusService
    {
        private readonly ApplicationDbContext _context;

        public ReservationStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReservationStatus>> GetAllAsync()
        {
            return await _context.ReservationStatuses.AsNoTracking().ToListAsync();
        }

        public async Task<ReservationStatus?> GetByIdAsync(Guid id)
        {
            return await _context.ReservationStatuses.FindAsync(id);
        }

        public async Task<ReservationStatus> CreateAsync(ReservationStatus entity)
        {
            entity.Id = Guid.NewGuid();
            _context.ReservationStatuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(ReservationStatus entity)
        {
            var existing = await _context.ReservationStatuses.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.ReservationStatuses.FindAsync(id);
            if (existing == null) return false;

            _context.ReservationStatuses.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
