using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zalagaonica.Application.DTOs;

namespace Application.Services
{
    public class RevenueService
    {
        private readonly ApplicationDbContext _context;

        public RevenueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RevenueDto>> GetAllAsync()
        {
            return await _context.Revenues
                .Include(r => r.Client)
                .Include(r => r.Employee)
                .Select(r => new RevenueDto
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Source = r.Source,
                    Description = r.Description,
                    Date = r.Date,
                    ClientName = r.Client != null ? r.Client.Name : null,
                    EmployeeName = r.Employee != null ? r.Employee.FullName : null
                })
                .ToListAsync();
        }

        public async Task<RevenueDto?> GetByIdAsync(Guid id)
        {
            return await _context.Revenues
                .Include(r => r.Client)
                .Include(r => r.Employee)
                .Where(r => r.Id == id)
                .Select(r => new RevenueDto
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Source = r.Source,
                    Description = r.Description,
                    Date = r.Date,
                    ClientName = r.Client != null ? r.Client.Name : null,
                    EmployeeName = r.Employee != null ? r.Employee.FullName : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Revenue> CreateAsync(Revenue entity)
        {
            _context.Revenues.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Guid id, Revenue updated)
        {
            var existing = await _context.Revenues.FindAsync(id);
            if (existing == null) return false;

            existing.Amount = updated.Amount;
            existing.Source = updated.Source;
            existing.Description = updated.Description;
            existing.Date = updated.Date;
            existing.ClientId = updated.ClientId;
            existing.EmployeeId = updated.EmployeeId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Revenues.FindAsync(id);
            if (entity == null) return false;

            _context.Revenues.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
