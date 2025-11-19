using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CashRegisterTransactionService
{
    private readonly ApplicationDbContext _context;

    public CashRegisterTransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CashRegisterTransaction>> GetAllAsync()
    {
        return await _context.CashRegisterTransactions.AsNoTracking().ToListAsync();
    }

    public async Task<CashRegisterTransaction?> GetByIdAsync(Guid id)
    {
        return await _context.CashRegisterTransactions.FindAsync(id);
    }

    public async Task<CashRegisterTransaction> CreateAsync(CashRegisterTransaction entity)
    {
        entity.Id = Guid.NewGuid();
        _context.CashRegisterTransactions.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(CashRegisterTransaction entity)
    {
        var existing = await _context.CashRegisterTransactions.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.CashRegisterTransactions.FindAsync(id);
        if (existing == null) return false;

        _context.CashRegisterTransactions.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
