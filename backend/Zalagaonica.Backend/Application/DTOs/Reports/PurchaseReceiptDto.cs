namespace Application.DTOs.Reports
{
    public class PurchaseReceiptDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public SellerDto Seller { get; set; } = new();
        public List<PurchaseItemDto> Items { get; set; } = new();
        public string Warehouse { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
    }

    public class SellerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Oib { get; set; } = string.Empty;
    }

    public class PurchaseItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal Mpc { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
