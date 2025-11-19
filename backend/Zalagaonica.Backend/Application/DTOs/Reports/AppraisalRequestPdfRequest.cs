using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Reports
{
    public class AppraisalRequestPdfRequest
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        public string? ClientAddress { get; set; }
        public string? ClientPhoneNumber { get; set; }

        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public string ItemDescription { get; set; } = string.Empty;

        [Required]
        public DateTime RequestDate { get; set; }

        public string? Notes { get; set; }
    }
}
