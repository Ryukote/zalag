using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class FileUploadService
{
    private readonly ApplicationDbContext _context;

    public FileUploadService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileUpload>> GetAllAsync()
    {
        return await _context.FileUploads.AsNoTracking().ToListAsync();
    }

    public async Task<FileUpload?> GetByIdAsync(Guid id)
    {
        return await _context.FileUploads.FindAsync(id);
    }

    public async Task<FileUpload> CreateAsync(FileUpload entity)
    {
        entity.Id = Guid.NewGuid();
        _context.FileUploads.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(FileUpload entity)
    {
        var existing = await _context.FileUploads.FindAsync(entity.Id);
        if (existing == null) return false;
        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.FileUploads.FindAsync(id);
        if (existing == null) return false;
        _context.FileUploads.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
