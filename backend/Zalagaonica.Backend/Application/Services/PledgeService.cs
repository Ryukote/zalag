using Application.DTOs.Inventory;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Services
{
    public class PledgeService
    {
        private readonly ApplicationDbContext _context;

        public PledgeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PledgeDto>> GetAllAsync()
        {
            var pledges = await _context.Pledges
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return pledges.Select(MapToDto).ToList();
        }

        public async Task<PledgeDto?> GetByIdAsync(Guid id)
        {
            var pledge = await _context.Pledges
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return pledge == null ? null : MapToDto(pledge);
        }

        public async Task<PledgeDto> CreateAsync(CreatePledgeDto dto)
        {
            var pledge = new Pledge
            {
                ClientId = dto.ClientId,
                ClientName = dto.ClientName,
                ItemName = dto.ItemName,
                ItemDescription = dto.ItemDescription,
                EstimatedValue = dto.EstimatedValue,
                LoanAmount = dto.LoanAmount,
                ReturnAmount = dto.ReturnAmount,
                Period = dto.Period,
                PledgeDate = DateTime.UtcNow,
                RedeemDeadline = DateTime.UtcNow.AddDays(dto.Period),
                ItemImagesJson = JsonSerializer.Serialize(dto.ItemImages),
                WarrantyFilesJson = JsonSerializer.Serialize(dto.WarrantyFiles),
                Redeemed = false,
                Forfeited = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Pledges.Add(pledge);
            await _context.SaveChangesAsync();

            return MapToDto(pledge);
        }

        public async Task<bool> UpdateAsync(UpdatePledgeDto dto)
        {
            var pledge = await _context.Pledges.FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (pledge == null)
                return false;

            if (dto.LoanAmount.HasValue)
                pledge.LoanAmount = dto.LoanAmount.Value;

            if (dto.ReturnAmount.HasValue)
                pledge.ReturnAmount = dto.ReturnAmount.Value;

            if (dto.Period.HasValue)
            {
                pledge.Period = dto.Period.Value;
                pledge.RedeemDeadline = pledge.PledgeDate.AddDays(dto.Period.Value);
            }

            if (dto.RedeemDeadline.HasValue)
                pledge.RedeemDeadline = dto.RedeemDeadline.Value;

            pledge.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RedeemAsync(Guid id)
        {
            var pledge = await _context.Pledges.FirstOrDefaultAsync(p => p.Id == id);

            if (pledge == null || pledge.Redeemed || pledge.Forfeited)
                return false;

            pledge.Redeemed = true;
            pledge.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ForfeitAsync(Guid id)
        {
            var pledge = await _context.Pledges.FirstOrDefaultAsync(p => p.Id == id);

            if (pledge == null || pledge.Redeemed || pledge.Forfeited)
                return false;

            pledge.Forfeited = true;
            pledge.UpdatedAt = DateTime.UtcNow;

            // Transfer item to main warehouse as Article
            var mainWarehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Type == "main" || w.Name.ToLower().Contains("glavno"));

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Name = pledge.ItemName,
                Description = pledge.ItemDescription ?? string.Empty,
                PurchasePrice = pledge.LoanAmount,
                RetailPrice = pledge.EstimatedValue,
                SalePrice = pledge.EstimatedValue,
                TaxRate = 25m, // Croatian standard VAT rate
                Stock = 1,
                Status = "available",
                WarehouseType = "main",
                WarehouseId = mainWarehouse?.Id,
                Group = "Forfeited Pledges",
                SupplierName = pledge.ClientName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Articles.Add(article);

            // Create warehouse card entry for tracking
            var warehouseCard = new WarehouseCard
            {
                Id = Guid.NewGuid(),
                ArticleId = article.Id,
                ArticleName = article.Name,
                Date = DateTime.UtcNow,
                DocumentType = "Forfeit",
                DocumentNumber = $"FORF-{pledge.Id.ToString()[..8]}",
                InQuantity = 1,
                OutQuantity = 0,
                Balance = 1,
                Notes = $"Item forfeited from pledge. Client: {pledge.ClientName}",
                CreatedAt = DateTime.UtcNow
            };

            _context.WarehouseCards.Add(warehouseCard);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var pledge = await _context.Pledges.FirstOrDefaultAsync(p => p.Id == id);

            if (pledge == null)
                return false;

            _context.Pledges.Remove(pledge);
            await _context.SaveChangesAsync();
            return true;
        }

        private PledgeDto MapToDto(Pledge pledge)
        {
            List<string> itemImages;
            List<string> warrantyFiles;

            try
            {
                itemImages = JsonSerializer.Deserialize<List<string>>(pledge.ItemImagesJson) ?? new List<string>();
            }
            catch
            {
                itemImages = new List<string>();
            }

            try
            {
                warrantyFiles = JsonSerializer.Deserialize<List<string>>(pledge.WarrantyFilesJson) ?? new List<string>();
            }
            catch
            {
                warrantyFiles = new List<string>();
            }

            return new PledgeDto
            {
                Id = pledge.Id,
                ClientId = pledge.ClientId,
                ClientName = pledge.ClientName,
                ItemName = pledge.ItemName,
                ItemDescription = pledge.ItemDescription,
                EstimatedValue = pledge.EstimatedValue,
                LoanAmount = pledge.LoanAmount,
                ReturnAmount = pledge.ReturnAmount,
                Period = pledge.Period,
                PledgeDate = pledge.PledgeDate,
                RedeemDeadline = pledge.RedeemDeadline,
                Redeemed = pledge.Redeemed,
                Forfeited = pledge.Forfeited,
                ItemImages = itemImages,
                WarrantyFiles = warrantyFiles
            };
        }
    }
}
