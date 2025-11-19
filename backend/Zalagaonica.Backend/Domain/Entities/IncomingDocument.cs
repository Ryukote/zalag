using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class IncomingDocument
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string DocumentNumber { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? SupplierName { get; set; }

        public DateTime DateReceived { get; set; } = DateTime.UtcNow;

        [MaxLength(300)]
        public string? Description { get; set; }

        // Extended fields to match frontend requirements
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [Range(-999999999, 999999999)]
        public decimal PurchaseValue { get; set; }

        [Range(-999999999, 999999999)]
        public decimal Margin { get; set; }

        [Range(0, 100)]
        public decimal Tax { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "otvoren"; // 'otvoren' or 'proknjižen'

        [MaxLength(200)]
        public string? WarehouseName { get; set; }

        [MaxLength(50)]
        public string DocumentType { get; set; } = "PRIMKA";

        public int Year { get; set; }

        [MaxLength(100)]
        public string? Operator { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsPosted { get; set; } = false;

        public decimal InvoiceValue { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }

        public decimal Cost { get; set; }

        public decimal WholesaleValue { get; set; }

        public decimal VatAmount { get; set; }

        public decimal RetailValue { get; set; }

        public decimal ReturnFee { get; set; }

        public decimal TotalWithReturnFee { get; set; }

        public decimal PretaxAmount { get; set; }

        public decimal TotalPaid { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
