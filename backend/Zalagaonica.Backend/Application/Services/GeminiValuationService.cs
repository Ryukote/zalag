using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class GeminiValuationService
{
    private readonly ApplicationDbContext _context;

    public GeminiValuationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GeminiValuation>> GetAllAsync()
    {
        return await _context.GeminiValuations.AsNoTracking().ToListAsync();
    }

    public async Task<GeminiValuation?> GetByIdAsync(Guid id)
    {
        return await _context.GeminiValuations.FindAsync(id);
    }

    public async Task<GeminiValuation> CreateAsync(GeminiValuation entity)
    {
        entity.Id = Guid.NewGuid();
        _context.GeminiValuations.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(GeminiValuation entity)
    {
        var existing = await _context.GeminiValuations.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.GeminiValuations.FindAsync(id);
        if (existing == null) return false;

        _context.GeminiValuations.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
