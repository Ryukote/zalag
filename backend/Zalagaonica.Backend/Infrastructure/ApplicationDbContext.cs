using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // 📦 CORE
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<WarehouseType> WarehouseTypes => Set<WarehouseType>();
        public DbSet<WarehouseCard> WarehouseCards => Set<WarehouseCard>();
        public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
        public DbSet<PriceChangeLog> PriceChangeLogs => Set<PriceChangeLog>();

        // 💰 FINANCIJE
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseRecord> PurchaseRecords => Set<PurchaseRecord>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<Pledge> Pledges => Set<Pledge>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<ReservationStatus> ReservationStatuses => Set<ReservationStatus>();
        public DbSet<Revenue> Revenues => Set<Revenue>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<LoanRepayment> LoanRepayments => Set<LoanRepayment>();
        public DbSet<Repayment> Repayments => Set<Repayment>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<CashRegisterTransaction> CashRegisterTransactions => Set<CashRegisterTransaction>();
        public DbSet<CustomerDebt> CustomerDebts => Set<CustomerDebt>();
        public DbSet<DailyClosing> DailyClosings => Set<DailyClosing>();
        public DbSet<ImportCalculation> ImportCalculations => Set<ImportCalculation>();
        public DbSet<InventoryBook> InventoryBooks => Set<InventoryBook>();
        public DbSet<InventoryCount> InventoryCounts => Set<InventoryCount>();
        public DbSet<Label> Labels => Set<Label>();
        public DbSet<DeliveryCost> DeliveryCosts => Set<DeliveryCost>();

        // 🧑‍💼 HR
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Payroll> Payrolls => Set<Payroll>();
        public DbSet<Vacation> Vacations => Set<Vacation>();

        // 🚗 VOZILA
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleStatus> VehicleStatuses => Set<VehicleStatus>();

        // 📄 Dokumentacija / AI
        public DbSet<FileUpload> FileUploads => Set<FileUpload>();
        public DbSet<IncomingDocument> IncomingDocuments => Set<IncomingDocument>();
        public DbSet<OutputDocument> OutputDocuments => Set<OutputDocument>();
        public DbSet<OutputDocumentItem> OutputDocumentItems => Set<OutputDocumentItem>();
        public DbSet<ItemData> ItemData => Set<ItemData>();
        public DbSet<GeminiValuation> GeminiValuations => Set<GeminiValuation>();

        // 🔐 Korisnici
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- CORE ----------
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Warehouse)
                .WithMany(w => w.Articles)
                .HasForeignKey(a => a.WarehouseId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.UnitOfMeasure)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Type)
                .WithMany(t => t.Warehouses)
                .HasForeignKey(w => w.WarehouseTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // ---------- REZERVACIJE ----------
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Article)
                .WithMany()
                .HasForeignKey(r => r.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Status)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.StatusId)
                .OnDelete(DeleteBehavior.SetNull);

            // ---------- FINANCIJE ----------
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Client)
                .WithMany()
                .HasForeignKey(l => l.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Article)
                .WithMany()
                .HasForeignKey(l => l.ArticleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LoanRepayment>()
                .HasOne(lr => lr.Loan)
                .WithMany(l => l.Repayments)
                .HasForeignKey(lr => lr.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Article)
                .WithMany()
                .HasForeignKey(s => s.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Article)
                .WithMany()
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PurchaseRecord>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pledge>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Pledges)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Revenue>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            // ---------- USER / ROLES ----------
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.Employee)
                .WithOne()
                .HasForeignKey<UserAccount>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
