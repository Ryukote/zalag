using Domain.Entities;

namespace Application.DTOs
{
    public class VacationRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VacationType Type { get; set; }
        public string? Reason { get; set; }
    }

    public class VacationApprovalDto
    {
        public bool Approved { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class VacationResponseDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VacationStatus Status { get; set; }
        public VacationType Type { get; set; }
        public DateTime RequestDate { get; set; }
        public string? Reason { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectionReason { get; set; }
        public int TotalDays { get; set; }
    }
}
