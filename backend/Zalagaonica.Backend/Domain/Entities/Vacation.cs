using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum VacationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum VacationType
    {
        AnnualLeave,      // godišnji odmor
        SickLeave,        // bolovanje
        PaidLeave         // plaćeni dopust
    }

    public class Vacation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public VacationStatus Status { get; set; } = VacationStatus.Pending;

        [Required]
        public VacationType Type { get; set; } = VacationType.AnnualLeave;

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public string? Reason { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string? RejectionReason { get; set; }

        public int TotalDays => (EndDate - StartDate).Days + 1;
    }
}
