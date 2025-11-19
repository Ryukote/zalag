using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OutputDocumentItemService
    {
        private readonly ApplicationDbContext _context;
        public OutputDocumentItemService(ApplicationDbContext context) => _context = context;

        public async Task<List<OutputDocumentItem>> GetAllAsync() =>
            await _context.OutputDocumentItems.AsNoTracking().ToListAsync();

        public async Task<OutputDocumentItem?> GetByIdAsync(Guid id) =>
            await _context.OutputDocumentItems.FindAsync(id);

        public async Task<OutputDocumentItem> CreateAsync(OutputDocumentItem entity)
        {
            entity.Id = Guid.NewGuid();
            _context.OutputDocumentItems.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(OutputDocumentItem entity)
        {
            var existing = await _context.OutputDocumentItems.FindAsync(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.OutputDocumentItems.FindAsync(id);
            if (existing == null) return false;
            _context.OutputDocumentItems.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
