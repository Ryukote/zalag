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

            // TODO: Transfer item to main warehouse as Article

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
