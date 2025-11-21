using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Services
{
    public class BookkeepingReportServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly BookkeepingReportService _service;

        public BookkeepingReportServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new BookkeepingReportService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GenerateMonthlyReportAsync_ShouldCalculateTotalsCorrectly()
        {
            // Arrange
            var year = 2024;
            var month = 11;
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Add test sales
            _context.Sales.AddRange(
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleDate = startDate.AddDays(5),
                    TotalAmount = 1000m,
                    ArticleId = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = 1000m
                },
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleDate = startDate.AddDays(10),
                    TotalAmount = 500m,
                    ArticleId = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = 500m
                }
            );

            // Add test pledges
            _context.Pledges.AddRange(
                new Pledge
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    StartDate = startDate.AddDays(3),
                    EndDate = startDate.AddDays(33),
                    LoanAmount = 2000m,
                    EstimatedValue = 3000m,
                    InterestRate = 5m,
                    ItemName = "Test Item",
                    Status = "aktivan"
                }
            );

            // Add test expenses
            _context.Expenses.Add(new Expense
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 300m,
                ExpenseDate = startDate.AddDays(7),
                Description = "Test Expense",
                Category = "Utilities"
            });

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateMonthlyReportAsync(year, month);

            // Assert
            report.Should().NotBeNull();
            report.Year.Should().Be(year);
            report.Month.Should().Be(month);
            report.TotalSales.Should().Be(1500m); // 1000 + 500
            report.TotalSalesWithTax.Should().Be(1875m); // 1500 * 1.25
            report.TotalTax.Should().Be(375m); // 1875 - 1500
            report.TotalPledgeAmount.Should().Be(2000m);
            report.TotalExpenses.Should().Be(300m);
            report.SalesCount.Should().Be(2);
            report.PledgesCount.Should().Be(1);
            report.ExpensesCount.Should().Be(1);
        }

        [Fact]
        public async Task GenerateMonthlyReportAsync_ShouldExcludeSalesOutsideDateRange()
        {
            // Arrange
            var year = 2024;
            var month = 11;
            var startDate = new DateTime(year, month, 1);

            // Sale inside range
            _context.Sales.Add(new Sale
            {
                Id = Guid.NewGuid().ToString(),
                SaleDate = startDate.AddDays(5),
                TotalAmount = 1000m,
                ArticleId = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                Quantity = 1,
                UnitPrice = 1000m
            });

            // Sale outside range (previous month)
            _context.Sales.Add(new Sale
            {
                Id = Guid.NewGuid().ToString(),
                SaleDate = startDate.AddMonths(-1),
                TotalAmount = 500m,
                ArticleId = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                Quantity = 1,
                UnitPrice = 500m
            });

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateMonthlyReportAsync(year, month);

            // Assert
            report.TotalSales.Should().Be(1000m);
            report.SalesCount.Should().Be(1);
        }

        [Fact]
        public async Task GenerateKpoReportAsync_ShouldCalculateInventoryTotals()
        {
            // Arrange
            var startDate = new DateTime(2024, 11, 1);
            var endDate = new DateTime(2024, 11, 30);

            _context.InventoryBooks.AddRange(
                new InventoryBook
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = startDate.AddDays(5),
                    ArticleName = "Test Article 1",
                    DocumentNumber = "DOC001",
                    InQuantity = 100,
                    OutQuantity = 0,
                    Balance = 100
                },
                new InventoryBook
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = startDate.AddDays(10),
                    ArticleName = "Test Article 1",
                    DocumentNumber = "DOC002",
                    InQuantity = 50,
                    OutQuantity = 30,
                    Balance = 120
                }
            );

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateKpoReportAsync(startDate, endDate);

            // Assert
            report.Should().NotBeNull();
            report.TotalIn.Should().Be(150); // 100 + 50
            report.TotalOut.Should().Be(30);
            report.FinalBalance.Should().Be(120);
            report.Entries.Should().HaveCount(2);
        }

        [Fact]
        public async Task GeneratePledgeReportAsync_ShouldGroupPledgesByStatus()
        {
            // Arrange
            var startDate = new DateTime(2024, 11, 1);
            var endDate = new DateTime(2024, 11, 30);
            var clientId = Guid.NewGuid().ToString();

            _context.Clients.Add(new Client
            {
                Id = clientId,
                Name = "Test Client",
                Oib = "12345678901"
            });

            _context.Pledges.AddRange(
                new Pledge
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = clientId,
                    StartDate = startDate.AddDays(1),
                    EndDate = startDate.AddDays(31),
                    LoanAmount = 1000m,
                    EstimatedValue = 1500m,
                    InterestRate = 5m,
                    ItemName = "Gold Ring",
                    Status = "aktivan"
                },
                new Pledge
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = clientId,
                    StartDate = startDate.AddDays(5),
                    EndDate = startDate.AddDays(35),
                    LoanAmount = 2000m,
                    EstimatedValue = 3000m,
                    InterestRate = 5m,
                    ItemName = "Gold Necklace",
                    Status = "otkupljen"
                },
                new Pledge
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = clientId,
                    StartDate = startDate.AddDays(10),
                    EndDate = startDate.AddDays(40),
                    LoanAmount = 500m,
                    EstimatedValue = 800m,
                    InterestRate = 5m,
                    ItemName = "Silver Bracelet",
                    Status = "istekao"
                }
            );

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GeneratePledgeReportAsync(startDate, endDate);

            // Assert
            report.Should().NotBeNull();
            report.TotalPledges.Should().Be(3);
            report.ActivePledges.Should().Be(1);
            report.RedeemedPledges.Should().Be(1);
            report.ExpiredPledges.Should().Be(1);
            report.ForfeitedPledges.Should().Be(0);
            report.TotalLoanAmount.Should().Be(3500m); // 1000 + 2000 + 500
            report.TotalEstimatedValue.Should().Be(5300m); // 1500 + 3000 + 800
            report.Pledges.Should().HaveCount(3);
        }

        [Fact]
        public async Task GenerateTaxReportAsync_ShouldCalculatePDVCorrectly()
        {
            // Arrange
            var year = 2024;
            var month = 11;
            var startDate = new DateTime(year, month, 1);

            _context.Sales.AddRange(
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleDate = startDate.AddDays(5),
                    TotalAmount = 1000m,
                    ArticleId = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = 1000m
                },
                new Sale
                {
                    Id = Guid.NewGuid().ToString(),
                    SaleDate = startDate.AddDays(15),
                    TotalAmount = 2000m,
                    ArticleId = Guid.NewGuid().ToString(),
                    ClientId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = 2000m
                }
            );

            _context.Pledges.Add(new Pledge
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                StartDate = startDate.AddDays(3),
                EndDate = startDate.AddDays(33),
                LoanAmount = 500m,
                EstimatedValue = 800m,
                InterestRate = 5m,
                ItemName = "Test Item",
                Status = "aktivan"
            });

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateTaxReportAsync(year, month);

            // Assert
            report.Should().NotBeNull();
            report.TotalSalesWithoutTax.Should().Be(3000m); // 1000 + 2000
            report.TaxRate.Should().Be(0.25m); // 25% PDV in Croatia
            report.TotalTax.Should().Be(750m); // 3000 * 0.25
            report.TotalSalesWithTax.Should().Be(3750m); // 3000 + 750
            report.TotalPurchases.Should().Be(500m);
            report.TaxableBase.Should().Be(2500m); // 3000 - 500
        }

        [Fact]
        public async Task GenerateMonthlyReportAsync_WithNoData_ShouldReturnZeroTotals()
        {
            // Arrange
            var year = 2024;
            var month = 11;

            // Act
            var report = await _service.GenerateMonthlyReportAsync(year, month);

            // Assert
            report.Should().NotBeNull();
            report.TotalSales.Should().Be(0);
            report.TotalSalesWithTax.Should().Be(0);
            report.TotalTax.Should().Be(0);
            report.TotalPledgeAmount.Should().Be(0);
            report.TotalExpenses.Should().Be(0);
            report.SalesCount.Should().Be(0);
            report.PledgesCount.Should().Be(0);
        }

        [Fact]
        public async Task GeneratePledgeReportAsync_ShouldIncludeClientDetails()
        {
            // Arrange
            var startDate = new DateTime(2024, 11, 1);
            var endDate = new DateTime(2024, 11, 30);
            var clientId = Guid.NewGuid().ToString();

            _context.Clients.Add(new Client
            {
                Id = clientId,
                Name = "Ivan Horvat",
                Oib = "12345678901",
                Address = "Test Address 123",
                City = "Zagreb"
            });

            _context.Pledges.Add(new Pledge
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = clientId,
                StartDate = startDate.AddDays(5),
                EndDate = startDate.AddDays(35),
                LoanAmount = 1000m,
                EstimatedValue = 1500m,
                InterestRate = 5m,
                ItemName = "Gold Ring",
                ItemDescription = "18K Gold Ring",
                Status = "aktivan",
                Weight = 10.5m,
                Fineness = 750
            });

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GeneratePledgeReportAsync(startDate, endDate);

            // Assert
            report.Pledges.Should().HaveCount(1);
            var pledgeDetail = report.Pledges.First();
            pledgeDetail.ClientName.Should().Be("Ivan Horvat");
            pledgeDetail.ClientOib.Should().Be("12345678901");
            pledgeDetail.ItemName.Should().Be("Gold Ring");
            pledgeDetail.ItemDescription.Should().Be("18K Gold Ring");
            pledgeDetail.Weight.Should().Be(10.5m);
            pledgeDetail.Fineness.Should().Be(750);
        }
    }
}
