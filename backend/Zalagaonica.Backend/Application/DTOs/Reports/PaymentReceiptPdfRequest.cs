using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Reports
{
    public class PaymentReceiptPdfRequest
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        public string ClientAddress { get; set; } = string.Empty;

        public string? ClientPhoneNumber { get; set; }

        [Required]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }
    }
}
