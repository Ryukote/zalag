using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CustomerDebtService
    {
        private readonly ApplicationDbContext _context;

        public CustomerDebtService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDebt>> GetAllAsync()
        {
            return await _context.CustomerDebts
                .OrderByDescending(cd => cd.DueDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CustomerDebt>> GetOverdueAsync()
        {
            return await _context.CustomerDebts
                .Where(cd => cd.DueDate < DateTime.UtcNow && cd.Remaining > 0)
                .OrderBy(cd => cd.DueDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CustomerDebt?> GetByIdAsync(Guid id)
        {
            return await _context.CustomerDebts.FindAsync(id);
        }

        public async Task<CustomerDebt> CreateAsync(CustomerDebt entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Status = GetStatus(entity.DueDate, entity.Remaining);
            entity.CreatedAt = DateTime.UtcNow;

            _context.CustomerDebts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(CustomerDebt entity)
        {
            var existing = await _context.CustomerDebts.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.CustomerName = entity.CustomerName;
            existing.CustomerCode = entity.CustomerCode;
            existing.TotalDebt = entity.TotalDebt;
            existing.Paid = entity.Paid;
            existing.DueDate = entity.DueDate;
            existing.Status = GetStatus(entity.DueDate, existing.Remaining);
            existing.Notes = entity.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.CustomerDebts.FindAsync(id);
            if (existing == null) return false;

            _context.CustomerDebts.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GetStatus(DateTime dueDate, decimal remaining)
        {
            if (remaining <= 0) return "paid";
            if (DateTime.UtcNow <= dueDate) return "current";

            var daysPastDue = (DateTime.UtcNow - dueDate).Days;
            if (daysPastDue > 30) return "critical";
            return "overdue";
        }
    }
}
