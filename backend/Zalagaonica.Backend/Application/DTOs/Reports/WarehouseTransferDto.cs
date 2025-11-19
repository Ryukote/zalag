namespace Application.DTOs.Reports
{
    public class WarehouseTransferDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string FromWarehouse { get; set; } = string.Empty;
        public string ToWarehouse { get; set; } = string.Empty;
        public List<TransferItemDto> Items { get; set; } = new();
        public string EmployeeName { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class TransferItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
