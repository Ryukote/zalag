using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class DeliveryCostService
    {
        private readonly ApplicationDbContext _context;

        public DeliveryCostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryCost>> GetAllAsync()
        {
            return await _context.DeliveryCosts
                .OrderByDescending(d => d.Date)
                .ToListAsync();
        }

        public async Task<DeliveryCost?> GetByIdAsync(Guid id)
        {
            return await _context.DeliveryCosts
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DeliveryCost> CreateAsync(DeliveryCost deliveryCost)
        {
            deliveryCost.Id = Guid.NewGuid();
            deliveryCost.CreatedAt = DateTime.UtcNow;
            deliveryCost.UpdatedAt = DateTime.UtcNow;

            _context.DeliveryCosts.Add(deliveryCost);
            await _context.SaveChangesAsync();
            return deliveryCost;
        }

        public async Task<bool> UpdateAsync(DeliveryCost deliveryCost)
        {
            var existing = await _context.DeliveryCosts.FindAsync(deliveryCost.Id);
            if (existing == null) return false;

            existing.Date = deliveryCost.Date;
            existing.Courier = deliveryCost.Courier;
            existing.TrackingNumber = deliveryCost.TrackingNumber;
            existing.Description = deliveryCost.Description;
            existing.Cost = deliveryCost.Cost;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deliveryCost = await _context.DeliveryCosts.FindAsync(id);
            if (deliveryCost == null) return false;

            _context.DeliveryCosts.Remove(deliveryCost);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
