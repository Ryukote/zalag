using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UnifiedDocumentSearchService
    {
        private readonly ApplicationDbContext _context;

        public UnifiedDocumentSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnifiedSearchResultDto> SearchDocumentsAsync(DocumentSearchQuery query)
        {
            var results = new UnifiedSearchResultDto
            {
                Sales = new List<DocumentResultDto>(),
                Purchases = new List<DocumentResultDto>(),
                Pledges = new List<DocumentResultDto>(),
                PurchaseRecords = new List<DocumentResultDto>(),
                OutputDocuments = new List<DocumentResultDto>()
            };

            // Search Sales
            if (query.IncludeSales)
            {
                var salesQuery = _context.Sales.AsQueryable();

                if (!string.IsNullOrEmpty(query.ClientName))
                {
                    salesQuery = salesQuery.Where(s =>
                        s.CustomerName != null && s.CustomerName.ToLower().Contains(query.ClientName.ToLower()));
                }

                if (!string.IsNullOrEmpty(query.ClientOib))
                {
                    salesQuery = salesQuery.Where(s =>
                        s.CustomerId != null && s.CustomerId.ToString().Contains(query.ClientOib));
                }

                if (!string.IsNullOrEmpty(query.ArticleName))
                {
                    salesQuery = salesQuery.Where(s =>
                        s.Description != null && s.Description.ToLower().Contains(query.ArticleName.ToLower()));
                }

                if (query.DateFrom.HasValue)
                {
                    salesQuery = salesQuery.Where(s => s.Date >= query.DateFrom.Value);
                }

                if (query.DateTo.HasValue)
                {
                    salesQuery = salesQuery.Where(s => s.Date <= query.DateTo.Value);
                }

                var sales = await salesQuery
                    .OrderByDescending(s => s.Date)
                    .Take(100)
                    .ToListAsync();

                results.Sales = sales.Select(s => new DocumentResultDto
                {
                    Id = s.Id.ToString(),
                    Type = "Sale",
                    TypeDisplay = "Prodaja",
                    DocumentNumber = $"SALE-{s.Id.ToString()[..8]}",
                    Date = s.Date,
                    ClientName = s.CustomerName ?? "N/A",
                    ArticleName = s.Description ?? "N/A",
                    Amount = s.TotalAmount,
                    Status = "Completed"
                }).ToList();
            }

            // Search Purchases
            if (query.IncludePurchases)
            {
                var purchasesQuery = _context.Purchases.AsQueryable();

                if (!string.IsNullOrEmpty(query.ClientName))
                {
                    purchasesQuery = purchasesQuery.Where(p =>
                        p.ClientName != null && p.ClientName.ToLower().Contains(query.ClientName.ToLower()));
                }

                if (!string.IsNullOrEmpty(query.ArticleName))
                {
                    purchasesQuery = purchasesQuery.Where(p =>
                        p.ItemName != null && p.ItemName.ToLower().Contains(query.ArticleName.ToLower()));
                }

                if (query.DateFrom.HasValue)
                {
                    purchasesQuery = purchasesQuery.Where(p => p.Date >= query.DateFrom.Value);
                }

                if (query.DateTo.HasValue)
                {
                    purchasesQuery = purchasesQuery.Where(p => p.Date <= query.DateTo.Value);
                }

                var purchases = await purchasesQuery
                    .OrderByDescending(p => p.Date)
                    .Take(100)
                    .ToListAsync();

                results.Purchases = purchases.Select(p => new DocumentResultDto
                {
                    Id = p.Id.ToString(),
                    Type = "Purchase",
                    TypeDisplay = "Otkup",
                    DocumentNumber = $"PUR-{p.Id.ToString()[..8]}",
                    Date = p.Date,
                    ClientName = p.ClientName ?? "N/A",
                    ArticleName = p.ItemName ?? "N/A",
                    Amount = p.Amount,
                    Status = "Completed"
                }).ToList();
            }

            // Search Pledges
            if (query.IncludePledges)
            {
                var pledgesQuery = _context.Pledges.AsQueryable();

                if (!string.IsNullOrEmpty(query.ClientName))
                {
                    pledgesQuery = pledgesQuery.Where(p =>
                        p.ClientName.ToLower().Contains(query.ClientName.ToLower()));
                }

                if (!string.IsNullOrEmpty(query.ArticleName))
                {
                    pledgesQuery = pledgesQuery.Where(p =>
                        p.ItemName.ToLower().Contains(query.ArticleName.ToLower()));
                }

                if (query.DateFrom.HasValue)
                {
                    pledgesQuery = pledgesQuery.Where(p => p.PledgeDate >= query.DateFrom.Value);
                }

                if (query.DateTo.HasValue)
                {
                    pledgesQuery = pledgesQuery.Where(p => p.PledgeDate <= query.DateTo.Value);
                }

                var pledges = await pledgesQuery
                    .OrderByDescending(p => p.PledgeDate)
                    .Take(100)
                    .ToListAsync();

                results.Pledges = pledges.Select(p => new DocumentResultDto
                {
                    Id = p.Id.ToString(),
                    Type = "Pledge",
                    TypeDisplay = "Zalog",
                    DocumentNumber = $"PLG-{p.Id.ToString()[..8]}",
                    Date = p.PledgeDate,
                    ClientName = p.ClientName,
                    ArticleName = p.ItemName,
                    Amount = p.LoanAmount,
                    Status = p.Redeemed ? "Otkupljeno" : (p.Forfeited ? "Propadnuto" : "Aktivno")
                }).ToList();
            }

            // Search Purchase Records
            if (query.IncludePurchaseRecords)
            {
                var purchaseRecordsQuery = _context.PurchaseRecords.AsQueryable();

                if (!string.IsNullOrEmpty(query.ClientName))
                {
                    purchaseRecordsQuery = purchaseRecordsQuery.Where(pr =>
                        pr.ClientName.ToLower().Contains(query.ClientName.ToLower()));
                }

                if (!string.IsNullOrEmpty(query.ArticleName))
                {
                    purchaseRecordsQuery = purchaseRecordsQuery.Where(pr =>
                        pr.ItemName.ToLower().Contains(query.ArticleName.ToLower()));
                }

                if (query.DateFrom.HasValue)
                {
                    purchaseRecordsQuery = purchaseRecordsQuery.Where(pr => pr.PurchaseDate >= query.DateFrom.Value);
                }

                if (query.DateTo.HasValue)
                {
                    purchaseRecordsQuery = purchaseRecordsQuery.Where(pr => pr.PurchaseDate <= query.DateTo.Value);
                }

                var purchaseRecords = await purchaseRecordsQuery
                    .OrderByDescending(pr => pr.PurchaseDate)
                    .Take(100)
                    .ToListAsync();

                results.PurchaseRecords = purchaseRecords.Select(pr => new DocumentResultDto
                {
                    Id = pr.Id.ToString(),
                    Type = "PurchaseRecord",
                    TypeDisplay = "Otkupni zapis",
                    DocumentNumber = $"PR-{pr.Id.ToString()[..8]}",
                    Date = pr.PurchaseDate,
                    ClientName = pr.ClientName,
                    ArticleName = pr.ItemName,
                    Amount = pr.TotalAmount,
                    Status = "Completed"
                }).ToList();
            }

            // Search Output Documents
            if (query.IncludeOutputDocuments)
            {
                var outputDocsQuery = _context.OutputDocuments.AsQueryable();

                if (!string.IsNullOrEmpty(query.ClientName))
                {
                    outputDocsQuery = outputDocsQuery.Where(od =>
                        od.ClientName != null && od.ClientName.ToLower().Contains(query.ClientName.ToLower()));
                }

                if (!string.IsNullOrEmpty(query.ArticleName))
                {
                    outputDocsQuery = outputDocsQuery.Where(od =>
                        od.Notes != null && od.Notes.ToLower().Contains(query.ArticleName.ToLower()));
                }

                if (query.DateFrom.HasValue)
                {
                    outputDocsQuery = outputDocsQuery.Where(od => od.Date >= query.DateFrom.Value);
                }

                if (query.DateTo.HasValue)
                {
                    outputDocsQuery = outputDocsQuery.Where(od => od.Date <= query.DateTo.Value);
                }

                var outputDocs = await outputDocsQuery
                    .OrderByDescending(od => od.Date)
                    .Take(100)
                    .ToListAsync();

                results.OutputDocuments = outputDocs.Select(od => new DocumentResultDto
                {
                    Id = od.Id.ToString(),
                    Type = "OutputDocument",
                    TypeDisplay = "Izlazni dokument",
                    DocumentNumber = od.DocumentNumber ?? $"OUT-{od.Id.ToString()[..8]}",
                    Date = od.Date,
                    ClientName = od.ClientName ?? "N/A",
                    ArticleName = od.Notes ?? "N/A",
                    Amount = od.TotalValue,
                    Status = "Completed"
                }).ToList();
            }

            results.TotalResults = results.Sales.Count + results.Purchases.Count +
                                  results.Pledges.Count + results.PurchaseRecords.Count +
                                  results.OutputDocuments.Count;

            return results;
        }

        public async Task<List<QuickSearchResultDto>> QuickSearchAsync(string searchTerm)
        {
            var results = new List<QuickSearchResultDto>();

            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
            {
                return results;
            }

            var term = searchTerm.ToLower();

            // Search Clients
            var clients = await _context.Clients
                .Where(c => c.Name.ToLower().Contains(term) ||
                           (c.Oib != null && c.Oib.Contains(searchTerm)))
                .Take(10)
                .Select(c => new QuickSearchResultDto
                {
                    Id = c.Id.ToString(),
                    Type = "Client",
                    Title = c.Name,
                    Subtitle = c.Oib ?? "Nema OIB",
                    Icon = "user"
                })
                .ToListAsync();

            results.AddRange(clients);

            // Search Articles
            var articles = await _context.Articles
                .Where(a => a.Name.ToLower().Contains(term) ||
                           (a.Description != null && a.Description.ToLower().Contains(term)))
                .Take(10)
                .Select(a => new QuickSearchResultDto
                {
                    Id = a.Id.ToString(),
                    Type = "Article",
                    Title = a.Name,
                    Subtitle = $"Cijena: {a.SalePrice}€",
                    Icon = "tag"
                })
                .ToListAsync();

            results.AddRange(articles);

            // Search Pledges
            var pledges = await _context.Pledges
                .Where(p => p.ItemName.ToLower().Contains(term) ||
                           p.ClientName.ToLower().Contains(term))
                .Take(5)
                .Select(p => new QuickSearchResultDto
                {
                    Id = p.Id.ToString(),
                    Type = "Pledge",
                    Title = p.ItemName,
                    Subtitle = $"Klijent: {p.ClientName} | {p.LoanAmount}€",
                    Icon = "document"
                })
                .ToListAsync();

            results.AddRange(pledges);

            return results.Take(20).ToList();
        }
    }

    // DTOs
    public class DocumentSearchQuery
    {
        public string? ClientName { get; set; }
        public string? ClientOib { get; set; }
        public string? ArticleName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IncludeSales { get; set; } = true;
        public bool IncludePurchases { get; set; } = true;
        public bool IncludePledges { get; set; } = true;
        public bool IncludePurchaseRecords { get; set; } = true;
        public bool IncludeOutputDocuments { get; set; } = true;
    }

    public class UnifiedSearchResultDto
    {
        public int TotalResults { get; set; }
        public List<DocumentResultDto> Sales { get; set; } = new();
        public List<DocumentResultDto> Purchases { get; set; } = new();
        public List<DocumentResultDto> Pledges { get; set; } = new();
        public List<DocumentResultDto> PurchaseRecords { get; set; } = new();
        public List<DocumentResultDto> OutputDocuments { get; set; } = new();
    }

    public class DocumentResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeDisplay { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ArticleName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class QuickSearchResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
