using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees.AsNoTracking().ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<Employee> CreateAsync(Employee entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Employees.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Employee entity)
    {
        var existing = await _context.Employees.FindAsync(entity.Id);
        if (existing == null) return false;
        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Employees.FindAsync(id);
        if (existing == null) return false;
        _context.Employees.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
