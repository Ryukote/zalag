using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BookkeepingReportService
    {
        private readonly ApplicationDbContext _context;

        public BookkeepingReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mjesečni izvještaj prodaje za knjigovođu (po hrvatskom zakonu)
        public async Task<MonthlyBookkeepingReportDto> GenerateMonthlyReportAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Ukupna prodaja
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();

            var totalSales = sales.Sum(s => s.TotalAmount);
            var totalSalesWithTax = totalSales * 1.25m; // PDV 25%
            var totalTax = totalSalesWithTax - totalSales;

            // Ukupan otkup (pledges)
            var pledges = await _context.Pledges
                .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                .ToListAsync();

            var totalPledgeAmount = pledges.Sum(p => p.LoanAmount);

            // Ukupne otplate
            var repayments = await _context.LoanRepayments
                .Where(r => r.RepaymentDate >= startDate && r.RepaymentDate <= endDate)
                .ToListAsync();

            var totalRepayments = repayments.Sum(r => r.AmountPaid);
            var totalInterest = repayments.Sum(r => r.InterestPaid);

            // Rashodi
            var expenses = await _context.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .ToListAsync();

            var totalExpenses = expenses.Sum(e => e.Amount);

            // Dnevna zatvaranja
            var dailyClosings = await _context.DailyClosings
                .Where(d => d.Date >= startDate && d.Date <= endDate && d.IsClosed)
                .ToListAsync();

            var totalCashSales = dailyClosings.Sum(d => d.TotalSales);
            var totalCashExpenses = dailyClosings.Sum(d => d.TotalExpenses);

            return new MonthlyBookkeepingReportDto
            {
                Year = year,
                Month = month,
                PeriodStart = startDate,
                PeriodEnd = endDate,
                TotalSales = totalSales,
                TotalSalesWithTax = totalSalesWithTax,
                TotalTax = totalTax,
                TotalPledgeAmount = totalPledgeAmount,
                TotalRepayments = totalRepayments,
                TotalInterest = totalInterest,
                TotalExpenses = totalExpenses,
                TotalCashSales = totalCashSales,
                TotalCashExpenses = totalCashExpenses,
                NetIncome = totalSales + totalInterest - totalExpenses,
                SalesCount = sales.Count,
                PledgesCount = pledges.Count,
                RepaymentsCount = repayments.Count,
                ExpensesCount = expenses.Count,
                GeneratedAt = DateTime.UtcNow
            };
        }

        // KPO izvještaj (Knjiga Popisa Objekata) - za poreznu
        public async Task<KpoReportDto> GenerateKpoReportAsync(DateTime startDate, DateTime endDate)
        {
            var inventoryEntries = await _context.InventoryBooks
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .OrderBy(i => i.Date)
                .ToListAsync();

            var totalIn = inventoryEntries.Sum(i => i.InQuantity);
            var totalOut = inventoryEntries.Sum(i => i.OutQuantity);
            var finalBalance = inventoryEntries.LastOrDefault()?.Balance ?? 0;

            var entries = inventoryEntries.Select(i => new KpoEntryDto
            {
                Date = i.Date,
                ArticleName = i.ArticleName,
                DocumentNumber = i.DocumentNumber,
                InQuantity = i.InQuantity,
                OutQuantity = i.OutQuantity,
                Balance = i.Balance,
                Notes = i.Notes
            }).ToList();

            return new KpoReportDto
            {
                PeriodStart = startDate,
                PeriodEnd = endDate,
                TotalIn = totalIn,
                TotalOut = totalOut,
                FinalBalance = finalBalance,
                Entries = entries,
                GeneratedAt = DateTime.UtcNow
            };
        }

        // Izvještaj o zalozima (za evidenciju po zakonu o zalagaonicama)
        public async Task<PledgeReportDto> GeneratePledgeReportAsync(DateTime startDate, DateTime endDate)
        {
            var pledges = await _context.Pledges
                .Include(p => p.Client)
                .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                .OrderBy(p => p.StartDate)
                .ToListAsync();

            var activePledges = pledges.Count(p => p.Status == "aktivan");
            var expiredPledges = pledges.Count(p => p.Status == "istekao");
            var redeemedPledges = pledges.Count(p => p.Status == "otkupljen");
            var forfeitedPledges = pledges.Count(p => p.Status == "prodan");

            var totalLoanAmount = pledges.Sum(p => p.LoanAmount);
            var totalEstimatedValue = pledges.Sum(p => p.EstimatedValue);

            var pledgeDetails = pledges.Select(p => new PledgeDetailDto
            {
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ClientName = p.Client.Name,
                ClientOib = p.Client.Oib,
                ItemName = p.ItemName,
                ItemDescription = p.ItemDescription,
                EstimatedValue = p.EstimatedValue,
                LoanAmount = p.LoanAmount,
                InterestRate = p.InterestRate,
                Status = p.Status,
                Weight = p.Weight,
                Fineness = p.Fineness
            }).ToList();

            return new PledgeReportDto
            {
                PeriodStart = startDate,
                PeriodEnd = endDate,
                TotalPledges = pledges.Count,
                ActivePledges = activePledges,
                ExpiredPledges = expiredPledges,
                RedeemedPledges = redeemedPledges,
                ForfeitedPledges = forfeitedPledges,
                TotalLoanAmount = totalLoanAmount,
                TotalEstimatedValue = totalEstimatedValue,
                Pledges = pledgeDetails,
                GeneratedAt = DateTime.UtcNow
            };
        }

        // Izvještaj o prometima za Poreznu upravu
        public async Task<TaxReportDto> GenerateTaxReportAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Prodaja s PDV-om
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();

            var taxRate = 0.25m; // PDV 25% u Hrvatskoj
            var totalSalesWithoutTax = sales.Sum(s => s.TotalAmount);
            var totalTax = totalSalesWithoutTax * taxRate;
            var totalSalesWithTax = totalSalesWithoutTax + totalTax;

            // Otkup (nabava) - nije oporezivo
            var pledges = await _context.Pledges
                .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                .ToListAsync();

            var totalPurchases = pledges.Sum(p => p.LoanAmount);

            return new TaxReportDto
            {
                Year = year,
                Month = month,
                PeriodStart = startDate,
                PeriodEnd = endDate,
                TotalSalesWithoutTax = totalSalesWithoutTax,
                TaxRate = taxRate,
                TotalTax = totalTax,
                TotalSalesWithTax = totalSalesWithTax,
                TotalPurchases = totalPurchases,
                TaxableBase = totalSalesWithoutTax - totalPurchases,
                GeneratedAt = DateTime.UtcNow
            };
        }
    }

    // DTOs za izvještaje
    public class MonthlyBookkeepingReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalSalesWithTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalPledgeAmount { get; set; }
        public decimal TotalRepayments { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalCashSales { get; set; }
        public decimal TotalCashExpenses { get; set; }
        public decimal NetIncome { get; set; }
        public int SalesCount { get; set; }
        public int PledgesCount { get; set; }
        public int RepaymentsCount { get; set; }
        public int ExpensesCount { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class KpoReportDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int FinalBalance { get; set; }
        public List<KpoEntryDto> Entries { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public class KpoEntryDto
    {
        public DateTime Date { get; set; }
        public string ArticleName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public int InQuantity { get; set; }
        public int OutQuantity { get; set; }
        public int Balance { get; set; }
        public string? Notes { get; set; }
    }

    public class PledgeReportDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int TotalPledges { get; set; }
        public int ActivePledges { get; set; }
        public int ExpiredPledges { get; set; }
        public int RedeemedPledges { get; set; }
        public int ForfeitedPledges { get; set; }
        public decimal TotalLoanAmount { get; set; }
        public decimal TotalEstimatedValue { get; set; }
        public List<PledgeDetailDto> Pledges { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public class PledgeDetailDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientOib { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string? ItemDescription { get; set; }
        public decimal EstimatedValue { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
        public int? Fineness { get; set; }
    }

    public class TaxReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal TotalSalesWithoutTax { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalSalesWithTax { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal TaxableBase { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
