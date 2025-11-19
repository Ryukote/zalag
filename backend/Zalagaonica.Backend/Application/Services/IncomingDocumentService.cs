using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class IncomingDocumentService
{
    private readonly ApplicationDbContext _context;

    public IncomingDocumentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IncomingDocument>> GetAllAsync()
    {
        return await _context.IncomingDocuments.AsNoTracking().ToListAsync();
    }

    public async Task<IncomingDocument?> GetByIdAsync(Guid id)
    {
        return await _context.IncomingDocuments.FindAsync(id);
    }

    public async Task<IncomingDocument> CreateAsync(IncomingDocument entity)
    {
        entity.Id = Guid.NewGuid();
        _context.IncomingDocuments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(IncomingDocument entity)
    {
        var existing = await _context.IncomingDocuments.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.IncomingDocuments.FindAsync(id);
        if (existing == null) return false;

        _context.IncomingDocuments.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
