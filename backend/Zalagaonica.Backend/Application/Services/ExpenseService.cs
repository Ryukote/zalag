using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zalagaonica.Application.DTOs;

namespace Application.Services
{
    public class ExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseDto>> GetAllAsync()
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .OrderByDescending(e => e.Date)
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Type = e.Type,
                    Amount = e.Amount,
                    Description = e.Description,
                    Date = e.Date,
                    EmployeeName = e.Employee != null ? e.Employee.FullName : null
                })
                .ToListAsync();
        }

        public async Task<ExpenseDto?> GetByIdAsync(Guid id)
        {
            return await _context.Expenses
                .Include(e => e.Employee)
                .Where(e => e.Id == id)
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Type = e.Type,
                    Amount = e.Amount,
                    Description = e.Description,
                    Date = e.Date,
                    EmployeeName = e.Employee != null ? e.Employee.FullName : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Expense> CreateAsync(Expense entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Expense entity)
        {
            var existing = await _context.Expenses.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.Type = entity.Type;
            existing.Amount = entity.Amount;
            existing.Description = entity.Description;
            existing.Date = entity.Date;
            existing.EmployeeId = entity.EmployeeId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Expenses.FindAsync(id);
            if (entity == null) return false;

            _context.Expenses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
