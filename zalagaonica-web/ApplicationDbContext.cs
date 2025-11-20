using Microsoft.EntityFrameworkCore;
using Zalagaonica.Domain.Entities;

namespace Zalagaonica.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<CashRegisterTransaction> CashRegisterTransactions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDebt> CustomerDebts { get; set; }
        public DbSet<DailyClosing> DailyClosings { get; set; }
        public DbSet<DeliveryCost> DeliveryCosts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<GeminiValuation> GeminiValuations { get; set; }
        public DbSet<ImportCalculation> ImportCalculations { get; set; }
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<InventoryBook> InventoryBooks { get; set; }
        public DbSet<InventoryCount> InventoryCounts { get; set; }
        public DbSet<ItemData> ItemData { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRepayment> LoanRepayments { get; set; }
        public DbSet<OutputDocument> OutputDocuments { get; set; }
        public DbSet<OutputDocumentItem> OutputDocumentItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Pledge> Pledges { get; set; }
        public DbSet<PriceChangeLog> PriceChangeLogs { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseRecord> PurchaseRecords { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleStatus> VehicleStatuses { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseCard> WarehouseCards { get; set; }
        public DbSet<WarehouseType> WarehouseTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional model configurations here
        }
    }
}
