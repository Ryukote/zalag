using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zalagaonica.Application.DTOs;

public class RepaymentService
{
    private readonly ApplicationDbContext _context;

    public RepaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RepaymentDto>> GetAllAsync()
    {
        return await _context.LoanRepayments
            .Include(r => r.Loan)
            .ThenInclude(x => x.Client)
            .OrderByDescending(r => r.PaymentDate)
            .Select(r => new RepaymentDto
            {
                Id = r.Id,
                Amount = r.Amount,
                PaymentDate = r.PaymentDate,
                Note = r.Note,
                ClientName = !string.IsNullOrEmpty(r.Loan!.Client!.Name) ? r.Loan!.Client!.Name : null,
                EmployeeName = "",
                LoanId = r.LoanId
            })
            .ToListAsync();
    }

    public async Task<RepaymentDto?> GetByIdAsync(Guid id)
    {
        return await _context.LoanRepayments
            .Include(r => r.Loan)
            .ThenInclude(x => x.Client)
            .Select(r => new RepaymentDto
            {
                Id = r.Id,
                Amount = r.Amount,
                PaymentDate = r.PaymentDate,
                Note = r.Note,
                ClientName = !string.IsNullOrEmpty(r.Loan!.Client!.Name) ? r.Loan!.Client!.Name : null,
                EmployeeName = "",
                LoanId = r.LoanId
            })
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<LoanRepayment> CreateAsync(LoanRepayment entity)
    {
        _context.LoanRepayments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.LoanRepayments.FindAsync(id);
        if (entity == null) return false;

        _context.LoanRepayments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
