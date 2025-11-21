using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class LoanRepaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly LoanService _loanService;

        public LoanRepaymentService(ApplicationDbContext context, LoanService loanService)
        {
            _context = context;
            _loanService = loanService;
        }

        // Dodavanje uplate/kamate na kredit
        public async Task<LoanRepayment> AddRepaymentAsync(Guid loanId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Repayment amount must be positive.");

            var loan = await _loanService.GetByIdAsync(loanId);
            if (loan == null)
                throw new Exception("Loan not found.");

            decimal outstanding = await _loanService.GetOutstandingBalanceAsync(loanId);

            if (amount > outstanding)
                throw new Exception("Repayment amount exceeds outstanding loan balance.");

            var repayment = new LoanRepayment
            {
                LoanId = loanId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow
            };

            _context.LoanRepayments.Add(repayment);

            // Knjigovodstvena logika - primer:
            // Kreiranje finansijskog zapisa u glavnoj knjizi, blagajni ili drugom modulu,
            // na primer kreiranje CashRegisterTransaction
            var transaction = new CashRegisterTransaction
            {
                Amount = amount,
                Date = DateTime.UtcNow,
                Description = $"Loan repayment for loan {loanId}",
                Type = "LoanRepayment"
            };
            _context.CashRegisterTransactions.Add(transaction);

            await _context.SaveChangesAsync();

            return repayment;
        }


        // Dohvati sve uplate za dati kredit
        public async Task<List<LoanRepayment>> GetRepaymentsForLoanAsync(Guid loanId)
        {
            return await _context.LoanRepayments
                .Where(r => r.LoanId == loanId)
                .ToListAsync();
        }
    }
}
