using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class WarehouseService
{
    private readonly ApplicationDbContext _context;

    public WarehouseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await _context.Warehouses.AsNoTracking().ToListAsync();
    }

    public async Task<Warehouse?> GetByIdAsync(Guid id)
    {
        return await _context.Warehouses.FindAsync(id);
    }

    public async Task<Warehouse> CreateAsync(Warehouse entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Warehouses.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Warehouse entity)
    {
        var existing = await _context.Warehouses.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Warehouses.FindAsync(id);
        if (existing == null) return false;

        _context.Warehouses.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
