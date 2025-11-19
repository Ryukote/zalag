using Domain.Entities;

namespace Application.DTOs
{
    // Import Calculation DTOs
    public class ImportCalculationDto
    {
        public Guid Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EUR";
        public decimal ExchangeRate { get; set; }
        public decimal TotalInLocalCurrency { get; set; }
        public string Status { get; set; } = "draft";
        public string? Notes { get; set; }
    }

    public class CreateImportCalculationDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime DocumentDate { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EUR";
        public decimal ExchangeRate { get; set; }
        public string? Notes { get; set; }
    }

    // Label DTOs
    public class LabelDto
    {
        public Guid Id { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateLabelDto
    {
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public int Quantity { get; set; } = 1;
    }

    // Price Change Log DTOs
    public class PriceChangeLogDto
    {
        public Guid Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
    }

    public class CreatePriceChangeLogDto
    {
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
    }

    // Inventory Book DTOs
    public class InventoryBookDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string EntryNumber { get; set; } = string.Empty;
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalSale { get; set; }
        public string Cashier { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class CreateInventoryBookDto
    {
        public DateTime Date { get; set; }
        public string EntryNumber { get; set; } = string.Empty;
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal SalePrice { get; set; }
        public string Cashier { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // Warehouse Card DTOs
    public class WarehouseCardDto
    {
        public Guid Id { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int ReservedStock { get; set; }
        public int AvailableStock { get; set; }
        public string UnitOfMeasure { get; set; } = "kom";
        public DateTime LastMovement { get; set; }
    }

    public class CreateWarehouseCardDto
    {
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int ReservedStock { get; set; }
        public string UnitOfMeasure { get; set; } = "kom";
    }

    // Customer Debt DTOs
    public class CustomerDebtDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public decimal TotalDebt { get; set; }
        public decimal Paid { get; set; }
        public decimal Remaining { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysPastDue { get; set; }
        public string Status { get; set; } = "current";
        public string? Notes { get; set; }
    }

    public class CreateCustomerDebtDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public decimal TotalDebt { get; set; }
        public decimal Paid { get; set; } = 0;
        public DateTime DueDate { get; set; }
        public string? Notes { get; set; }
    }

    // Inventory Count DTOs
    public class InventoryCountDto
    {
        public Guid Id { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public int BookQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public int Difference { get; set; }
        public string Warehouse { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
        public string? Notes { get; set; }
    }

    public class CreateInventoryCountDto
    {
        public string InventoryNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ArticleCode { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public int BookQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public string Warehouse { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // Daily Closing DTOs
    public class DailyClosingDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public decimal StartingCash { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal CashInRegister { get; set; }
        public decimal ExpectedCash { get; set; }
        public decimal Difference { get; set; }
        public bool IsClosed { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateDailyClosingDto
    {
        public DateTime Date { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public decimal StartingCash { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal CashInRegister { get; set; }
        public string? Notes { get; set; }
    }
}
