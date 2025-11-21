using Application.DTOs.Inventory;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Services
{
    public class PurchaseRecordService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseRecordDto>> GetAllAsync()
        {
            var purchases = await _context.PurchaseRecords
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return purchases.Select(MapToDto).ToList();
        }

        public async Task<PurchaseRecordDto?> GetByIdAsync(Guid id)
        {
            var purchase = await _context.PurchaseRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return purchase == null ? null : MapToDto(purchase);
        }

        public async Task<PurchaseRecordDto> CreateAsync(CreatePurchaseRecordDto dto)
        {
            var purchase = new PurchaseRecord
            {
                ClientId = dto.ClientId,
                ClientName = dto.ClientName,
                ItemName = dto.ItemName,
                ItemDescription = dto.ItemDescription,
                EstimatedValue = dto.EstimatedValue,
                PurchaseAmount = dto.PurchaseAmount,
                TotalAmount = dto.TotalAmount,
                PurchaseDate = DateTime.UtcNow,
                PaymentDate = dto.PaymentDate,
                ItemImagesJson = JsonSerializer.Serialize(dto.ItemImages),
                WarrantyFilesJson = JsonSerializer.Serialize(dto.WarrantyFiles),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PurchaseRecords.Add(purchase);
            await _context.SaveChangesAsync();

            return MapToDto(purchase);
        }

        public async Task<bool> UpdateAsync(Guid id, CreatePurchaseRecordDto dto)
        {
            var purchase = await _context.PurchaseRecords.FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return false;

            purchase.ClientId = dto.ClientId;
            purchase.ClientName = dto.ClientName;
            purchase.ItemName = dto.ItemName;
            purchase.ItemDescription = dto.ItemDescription;
            purchase.EstimatedValue = dto.EstimatedValue;
            purchase.PurchaseAmount = dto.PurchaseAmount;
            purchase.TotalAmount = dto.TotalAmount;
            purchase.PaymentDate = dto.PaymentDate;
            purchase.ItemImagesJson = JsonSerializer.Serialize(dto.ItemImages);
            purchase.WarrantyFilesJson = JsonSerializer.Serialize(dto.WarrantyFiles);
            purchase.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var purchase = await _context.PurchaseRecords.FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return false;

            _context.PurchaseRecords.Remove(purchase);
            await _context.SaveChangesAsync();
            return true;
        }

        private PurchaseRecordDto MapToDto(PurchaseRecord purchase)
        {
            List<string> itemImages;
            List<string> warrantyFiles;

            try
            {
                itemImages = JsonSerializer.Deserialize<List<string>>(purchase.ItemImagesJson) ?? new List<string>();
            }
            catch
            {
                itemImages = new List<string>();
            }

            try
            {
                warrantyFiles = JsonSerializer.Deserialize<List<string>>(purchase.WarrantyFilesJson) ?? new List<string>();
            }
            catch
            {
                warrantyFiles = new List<string>();
            }

            return new PurchaseRecordDto
            {
                Id = purchase.Id,
                ClientId = purchase.ClientId,
                ClientName = purchase.ClientName,
                ItemName = purchase.ItemName,
                ItemDescription = purchase.ItemDescription,
                EstimatedValue = purchase.EstimatedValue,
                PurchaseAmount = purchase.PurchaseAmount,
                TotalAmount = purchase.TotalAmount,
                PurchaseDate = purchase.PurchaseDate,
                PaymentDate = purchase.PaymentDate,
                ItemImages = itemImages,
                WarrantyFiles = warrantyFiles
            };
        }
    }
}
