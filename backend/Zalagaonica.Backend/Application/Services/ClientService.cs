using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class ClientService
{
    private readonly ApplicationDbContext _context;

    public ClientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients.AsNoTracking().ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> CreateAsync(Client entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Clients.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Client entity)
    {
        var existing = await _context.Clients.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Clients.FindAsync(id);
        if (existing == null) return false;

        _context.Clients.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
