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

            // Add test pledges with correct field names
            _context.Pledges.Add(new Pledge
            {
                Id = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                ClientName = "Test Client",
                PledgeDate = startDate.AddDays(3),
                RedeemDeadline = startDate.AddDays(33),
                LoanAmount = 2000m,
                EstimatedValue = 3000m,
                ItemName = "Test Item",
                ItemDescription = "Test Description",
                Period = 30,
                ReturnAmount = 2200m
            });

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateMonthlyReportAsync(year, month);

            // Assert
            report.Should().NotBeNull();
            report.Year.Should().Be(year);
            report.Month.Should().Be(month);
            report.TotalSales.Should().Be(1500m);
            report.TotalSalesWithTax.Should().Be(1875m);
            report.TotalTax.Should().Be(375m);
            report.TotalPledgeAmount.Should().Be(2000m);
            report.SalesCount.Should().Be(2);
            report.PledgesCount.Should().Be(1);
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

            await _context.SaveChangesAsync();

            // Act
            var report = await _service.GenerateTaxReportAsync(year, month);

            // Assert
            report.Should().NotBeNull();
            report.TotalSalesWithoutTax.Should().Be(3000m);
            report.TaxRate.Should().Be(0.25m);
            report.TotalTax.Should().Be(750m);
            report.TotalSalesWithTax.Should().Be(3750m);
        }
    }
}
