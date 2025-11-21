using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class LoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loans.AsNoTracking().ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<Loan> CreateAsync(Loan entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Loans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Loan entity)
        {
            var existing = await _context.Loans.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Loans.FindAsync(id);
            if (existing == null) return false;

            _context.Loans.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        // Metoda za dohvat preostalog duga (outstanding balance) za određenog korisnika ili zajam
        public async Task<decimal> GetOutstandingBalanceAsync(Guid loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Repayments) // Pretpostavka: LoanRepayments kolekcija u Loan entitetu
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new ArgumentException("Loan not found", nameof(loanId));

            // Pretpostavimo da Loan ima polje Amount (ukupni iznos)
            // i LoanRepayments ima polje Amount (iznos otplate)
            var totalRepayments = loan.Repayments?.Sum(r => r.Amount) ?? 0m;

            return loan.Amount - totalRepayments;
        }
    }
}
