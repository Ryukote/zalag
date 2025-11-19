using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ImportCalculationService
    {
        private readonly ApplicationDbContext _context;

        public ImportCalculationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ImportCalculation>> GetAllAsync()
        {
            return await _context.ImportCalculations
                .OrderByDescending(ic => ic.DocumentDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ImportCalculation?> GetByIdAsync(Guid id)
        {
            return await _context.ImportCalculations.FindAsync(id);
        }

        public async Task<ImportCalculation> CreateAsync(ImportCalculation entity)
        {
            entity.Id = Guid.NewGuid();
            entity.TotalInLocalCurrency = entity.TotalAmount * entity.ExchangeRate;
            entity.CreatedAt = DateTime.UtcNow;

            _context.ImportCalculations.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(ImportCalculation entity)
        {
            var existing = await _context.ImportCalculations.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.DocumentNumber = entity.DocumentNumber;
            existing.DocumentDate = entity.DocumentDate;
            existing.SupplierName = entity.SupplierName;
            existing.TotalAmount = entity.TotalAmount;
            existing.Currency = entity.Currency;
            existing.ExchangeRate = entity.ExchangeRate;
            existing.TotalInLocalCurrency = entity.TotalAmount * entity.ExchangeRate;
            existing.Status = entity.Status;
            existing.Notes = entity.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.ImportCalculations.FindAsync(id);
            if (existing == null) return false;

            _context.ImportCalculations.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
