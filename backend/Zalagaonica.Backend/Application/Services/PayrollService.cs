using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class PayrollService
{
    private readonly ApplicationDbContext _context;

    public PayrollService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Payroll>> GetAllAsync()
    {
        return await _context.Payrolls.AsNoTracking().ToListAsync();
    }

    public async Task<Payroll?> GetByIdAsync(Guid id)
    {
        return await _context.Payrolls.FindAsync(id);
    }

    public async Task<Payroll> CreateAsync(Payroll entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Payrolls.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Payroll entity)
    {
        var existing = await _context.Payrolls.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Payrolls.FindAsync(id);
        if (existing == null) return false;

        _context.Payrolls.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
