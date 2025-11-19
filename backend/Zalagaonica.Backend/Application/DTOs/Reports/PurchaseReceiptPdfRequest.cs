using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Reports
{
    public class PurchaseReceiptPdfRequest
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        public string ClientAddress { get; set; } = string.Empty;

        [Required]
        public string ArticleName { get; set; } = string.Empty;

        [Required]
        public string ArticleDescription { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }
    }
}
