using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class SaleService
{
    private readonly ApplicationDbContext _context;

    public SaleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetAllAsync()
    {
        return await _context.Sales.AsNoTracking().ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales.FindAsync(id);
    }

    public async Task<Sale> CreateAsync(Sale entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Sales.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Sale entity)
    {
        var existing = await _context.Sales.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Sales.FindAsync(id);
        if (existing == null) return false;

        _context.Sales.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
