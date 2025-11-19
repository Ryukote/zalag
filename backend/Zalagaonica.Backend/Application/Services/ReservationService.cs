using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class ReservationService
{
    private readonly ApplicationDbContext _context;

    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _context.Reservations.AsNoTracking().ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task<Reservation> CreateAsync(Reservation entity)
    {
        entity.Id = Guid.NewGuid();
        _context.Reservations.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Reservation entity)
    {
        var existing = await _context.Reservations.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.Reservations.FindAsync(id);
        if (existing == null) return false;

        _context.Reservations.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
