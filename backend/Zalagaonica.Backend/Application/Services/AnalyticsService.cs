using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var thisYear = new DateTime(today.Year, 1, 1);

            var totalSales = await _context.Sales.CountAsync();
            var totalArticles = await _context.Articles.CountAsync();
            var totalClients = await _context.Clients.CountAsync();
            var activePledges = await _context.Pledges.CountAsync(p => !p.Redeemed && !p.Forfeited);

            var todaySales = await _context.Sales
                .Where(s => s.Date >= today)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

            var monthSales = await _context.Sales
                .Where(s => s.Date >= thisMonth)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

            var yearSales = await _context.Sales
                .Where(s => s.Date >= thisYear)
                .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

            return new DashboardStatsDto
            {
                TotalSales = totalSales,
                TotalArticles = totalArticles,
                TotalClients = totalClients,
                ActivePledges = activePledges,
                TodaySalesAmount = todaySales,
                MonthSalesAmount = monthSales,
                YearSalesAmount = yearSales
            };
        }

        public async Task<List<SalesChartDataDto>> GetSalesChartDataAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _context.Sales
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .GroupBy(s => s.Date.Date)
                .Select(g => new SalesChartDataDto
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(s => s.TotalAmount),
                    Count = g.Count()
                })
                .OrderBy(s => s.Date)
                .ToListAsync();

            return sales;
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int count = 10)
        {
            var topProducts = await _context.Articles
                .OrderByDescending(a => a.SaleInfoPrice ?? 0)
                .Take(count)
                .Select(a => new TopSellingProductDto
                {
                    ArticleId = a.Id,
                    Name = a.Name,
                    TotalSold = a.Stock > 0 ? 0 : 1,  // Simplified logic
                    TotalRevenue = a.SaleInfoPrice ?? 0
                })
                .ToListAsync();

            return topProducts;
        }

        public async Task<List<WarehouseStatsDto>> GetWarehouseStatsAsync()
        {
            var warehouses = await _context.Warehouses
                .Select(w => new WarehouseStatsDto
                {
                    WarehouseId = w.Id,
                    WarehouseName = w.Name,
                    TotalArticles = _context.Articles.Count(a => a.WarehouseId == w.Id),
                    TotalValue = _context.Articles
                        .Where(a => a.WarehouseId == w.Id)
                        .Sum(a => (decimal?)(a.PurchasePrice * a.Stock)) ?? 0
                })
                .ToListAsync();

            return warehouses;
        }

        public async Task<List<PledgeStatsDto>> GetPledgeStatsAsync()
        {
            var total = await _context.Pledges.CountAsync();
            var redeemed = await _context.Pledges.CountAsync(p => p.Redeemed);
            var forfeited = await _context.Pledges.CountAsync(p => p.Forfeited);
            var active = await _context.Pledges.CountAsync(p => !p.Redeemed && !p.Forfeited);

            return new List<PledgeStatsDto>
            {
                new PledgeStatsDto { Status = "Active", Count = active },
                new PledgeStatsDto { Status = "Redeemed", Count = redeemed },
                new PledgeStatsDto { Status = "Forfeited", Count = forfeited }
            };
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);

            var revenue = await _context.Sales
                .Where(s => s.Date >= startDate)
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(s => s.TotalAmount),
                    SalesCount = g.Count()
                })
                .OrderBy(r => r.Year).ThenBy(r => r.Month)
                .ToListAsync();

            return revenue;
        }

        public async Task<List<ClientStatsDto>> GetTopClientsAsync(int count = 10)
        {
            var topClients = await _context.Clients
                .Select(c => new ClientStatsDto
                {
                    ClientId = c.Id,
                    ClientName = c.Name,
                    TotalPurchases = _context.Purchases.Count(p => p.ClientId == c.Id),
                    TotalPledges = _context.Pledges.Count(p => p.ClientId == c.Id),
                    TotalSpent = _context.Sales.Where(s => s.CustomerId == c.Id).Sum(s => (decimal?)s.TotalAmount) ?? 0
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(count)
                .ToListAsync();

            return topClients;
        }
    }

    // DTOs for Analytics
    public class DashboardStatsDto
    {
        public int TotalSales { get; set; }
        public int TotalArticles { get; set; }
        public int TotalClients { get; set; }
        public int ActivePledges { get; set; }
        public decimal TodaySalesAmount { get; set; }
        public decimal MonthSalesAmount { get; set; }
        public decimal YearSalesAmount { get; set; }
    }

    public class SalesChartDataDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public int Count { get; set; }
    }

    public class TopSellingProductDto
    {
        public Guid ArticleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class WarehouseStatsDto
    {
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int TotalArticles { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class PledgeStatsDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public int SalesCount { get; set; }
    }

    public class ClientStatsDto
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int TotalPurchases { get; set; }
        public int TotalPledges { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
