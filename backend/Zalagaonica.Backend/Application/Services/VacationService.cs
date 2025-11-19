using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class VacationService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public VacationService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<Vacation>> GetAllAsync()
        {
            return await _context.Vacations
                .OrderByDescending(v => v.RequestDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Vacation>> GetPendingAsync()
        {
            return await _context.Vacations
                .Where(v => v.Status == VacationStatus.Pending)
                .OrderBy(v => v.RequestDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Vacation>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Vacations
                .Where(v => v.EmployeeId == employeeId)
                .OrderByDescending(v => v.RequestDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Vacation?> GetByIdAsync(Guid id)
        {
            return await _context.Vacations.FindAsync(id);
        }

        public async Task<Vacation> CreateAsync(Vacation entity)
        {
            entity.Id = Guid.NewGuid();
            entity.Status = VacationStatus.Pending;
            entity.RequestDate = DateTime.UtcNow;

            _context.Vacations.Add(entity);
            await _context.SaveChangesAsync();

            // Send notification to admin
            await _emailService.SendVacationRequestNotificationAsync(
                entity.EmployeeName,
                entity.StartDate,
                entity.EndDate,
                entity.TotalDays,
                entity.Reason
            );

            return entity;
        }

        public async Task<bool> ApproveAsync(Guid vacationId, Guid approvedByUserId)
        {
            var vacation = await _context.Vacations.FindAsync(vacationId);
            if (vacation == null) return false;

            vacation.Status = VacationStatus.Approved;
            vacation.ApprovedBy = approvedByUserId;
            vacation.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Get employee email for notification
            var employee = await _context.Employees.FindAsync(vacation.EmployeeId);
            if (employee != null)
            {
                await _emailService.SendVacationStatusUpdateAsync(
                    vacation.EmployeeName,
                    employee.Email ?? "",
                    vacation.StartDate,
                    vacation.EndDate,
                    true
                );
            }

            return true;
        }

        public async Task<bool> RejectAsync(Guid vacationId, Guid rejectedByUserId, string? rejectionReason)
        {
            var vacation = await _context.Vacations.FindAsync(vacationId);
            if (vacation == null) return false;

            vacation.Status = VacationStatus.Rejected;
            vacation.ApprovedBy = rejectedByUserId;
            vacation.ApprovedDate = DateTime.UtcNow;
            vacation.RejectionReason = rejectionReason;

            await _context.SaveChangesAsync();

            // Get employee email for notification
            var employee = await _context.Employees.FindAsync(vacation.EmployeeId);
            if (employee != null)
            {
                await _emailService.SendVacationStatusUpdateAsync(
                    vacation.EmployeeName,
                    employee.Email ?? "",
                    vacation.StartDate,
                    vacation.EndDate,
                    false,
                    rejectionReason
                );
            }

            return true;
        }

        public async Task<bool> UpdateAsync(Vacation entity)
        {
            var existing = await _context.Vacations.FindAsync(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Vacations.FindAsync(id);
            if (existing == null) return false;

            _context.Vacations.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
