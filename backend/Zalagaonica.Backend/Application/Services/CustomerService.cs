using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync() =>
            await _context.Customers.AsNoTracking().ToListAsync();

        public async Task<Customer?> GetByIdAsync(Guid id) =>
            await _context.Customers.FindAsync(id);

        public async Task<Customer> CreateAsync(Customer entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Customer entity)
        {
            var existing = await _context.Customers.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return false;

            _context.Customers.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
