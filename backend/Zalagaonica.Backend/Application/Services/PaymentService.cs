using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zalagaonica.Application.DTOs;

public class PaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentDto>> GetAllAsync()
    {
        return await _context.Payments
            .Include(p => p.Client)
            .Include(p => p.Employee)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Method = p.Method,
                Description = p.Description,
                Date = p.Date,
                ClientName = p.Client != null ? p.Client.Name : null,
                EmployeeName = p.Employee != null ? p.Employee.FullName : null
            })
            .ToListAsync();
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        return await _context.Payments
            .Include(p => p.Client)
            .Include(p => p.Employee)
            .Where(p => p.Id == id)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Method = p.Method,
                Description = p.Description,
                Date = p.Date,
                ClientName = p.Client != null ? p.Client.Name : null,
                EmployeeName = p.Employee != null ? p.Employee.FullName : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Payment> CreateAsync(Payment entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Payments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Guid id, Payment updated)
    {
        var existing = await _context.Payments.FindAsync(id);
        if (existing == null) return false;

        existing.Amount = updated.Amount;
        existing.Method = updated.Method;
        existing.Description = updated.Description;
        existing.Date = updated.Date;
        existing.ClientId = updated.ClientId;
        existing.EmployeeId = updated.EmployeeId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Payments.FindAsync(id);
        if (entity == null) return false;

        _context.Payments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
