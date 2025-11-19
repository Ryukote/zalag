using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OutputDocumentService
    {
        private readonly ApplicationDbContext _context;

        public OutputDocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OutputDocument>> GetAllAsync()
        {
            return await _context.OutputDocuments
                .OrderByDescending(d => d.DocumentDate)
                .ToListAsync();
        }

        public async Task<OutputDocument?> GetByIdAsync(Guid id)
        {
            return await _context.OutputDocuments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<OutputDocument> CreateAsync(OutputDocument document)
        {
            document.Id = Guid.NewGuid();
            document.CreatedAt = DateTime.UtcNow;
            document.UpdatedAt = DateTime.UtcNow;
            document.Year = document.DocumentDate.Year;

            _context.OutputDocuments.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<bool> UpdateAsync(OutputDocument document)
        {
            var existing = await _context.OutputDocuments.FindAsync(document.Id);
            if (existing == null) return false;

            existing.ClientName = document.ClientName;
            existing.DocumentNumber = document.DocumentNumber;
            existing.DocumentDate = document.DocumentDate;
            existing.TotalValue = document.TotalValue;
            existing.Status = document.Status;
            existing.DocumentType = document.DocumentType;
            existing.Year = document.DocumentDate.Year;
            existing.Operator = document.Operator;
            existing.Note = document.Note;
            existing.IsPosted = document.IsPosted;
            existing.TotalWithTax = document.TotalWithTax;
            existing.PretaxAmount = document.PretaxAmount;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var document = await _context.OutputDocuments.FindAsync(id);
            if (document == null) return false;

            _context.OutputDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
