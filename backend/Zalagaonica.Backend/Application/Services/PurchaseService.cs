using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class PurchaseService
{
    private readonly ApplicationDbContext _context;

    public PurchaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Purchase>> GetAllAsync()
    {
        return await _context.Purchases.AsNoTracking().ToListAsync();
    }

    public async Task<Purchase?> GetByIdAsync(Guid id)
    {
        return await _context.Purchases.FindAsync(id);
    }

    public async Task<Purchase> CreateAsync(Purchase entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Purchases.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Purchase entity)
    {
        var existing = await _context.Purchases.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Purchases.FindAsync(id);
        if (existing == null) return false;

        _context.Purchases.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
