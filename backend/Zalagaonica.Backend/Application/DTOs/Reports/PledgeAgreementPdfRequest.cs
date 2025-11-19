using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Reports
{
    public class PledgeAgreementPdfRequest
    {
        [Required]
        public string PledgeNumber { get; set; } = string.Empty;

        [Required]
        public DateTime PledgeDate { get; set; }

        [Required]
        public string ClientName { get; set; } = string.Empty;

        public string? ClientAddress { get; set; }
        public string? ClientCity { get; set; }
        public string? ClientOib { get; set; }

        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public string ItemDescription { get; set; } = string.Empty;

        [Required]
        public decimal EstimatedValue { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public decimal ReturnAmount { get; set; }

        [Required]
        public int Period { get; set; }

        [Required]
        public DateTime RedeemDeadline { get; set; }
    }
}
