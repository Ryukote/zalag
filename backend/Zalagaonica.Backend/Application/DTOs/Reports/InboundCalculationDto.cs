namespace Application.DTOs.Reports
{
    public class InboundCalculationDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string IncomingDocumentNumber { get; set; } = string.Empty;
        public DateTime IncomingDocumentDate { get; set; }
        public SupplierDto Seller { get; set; } = new();
        public string Warehouse { get; set; } = string.Empty;
        public List<InboundItemDto> Items { get; set; } = new();
        public decimal TotalInvoicePrice { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalMargin { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalRetailPrice { get; set; }
        public VatOnAddedValueDto VatOnAddedValue { get; set; } = new();
    }

    public class SupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string Oib { get; set; } = string.Empty;
    }

    public class InboundItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal InvoicePrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MarginPercent { get; set; }
        public decimal MarginAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal RetailPrice { get; set; }
    }

    public class VatOnAddedValueDto
    {
        public decimal Base { get; set; }
        public decimal Amount { get; set; }
    }
}
