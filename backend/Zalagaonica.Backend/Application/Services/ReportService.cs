using Application.Reports.Templates;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

public class ReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1. Otkupni blok izvještaj za artikl
    public async Task<byte[]> GenerateOtkupniBlokAsync(Guid articleId)
    {
        var article = await _context.Articles.Include(a => a.Warehouse).FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null) throw new Exception("Artikl nije pronađen.");

        var client = await _context.Clients.FirstOrDefaultAsync(); // može uz artikl vezan klijent
        var report = new OtkupniBlokReport(client!, article, 250m, $"OTK-{DateTime.Now:yyyyMMdd}", DateTime.Now);
        return report.GeneratePdf();
    }

    // 2. Zahtjev za procjenu
    public async Task<byte[]> GenerateZahtjevZaProcjenuAsync(Guid articleId)
    {
        var article = await _context.Articles.Include(a => a.Warehouse).FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null) throw new Exception("Artikl nije pronađen.");

        var client = await _context.Clients.FirstOrDefaultAsync();
        var report = new ZahtjevZaProcjenuReport(client!, article, "Procjena autentičnosti", $"PROC-{DateTime.Now:yyyyMMdd}", DateTime.Now);
        return report.GeneratePdf();
    }

    // 3. Međuskladišnica
    public async Task<byte[]> GenerateMedjuskladisnicaAsync(Guid articleId)
    {
        var article = await _context.Articles.Include(a => a.Warehouse).FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null) throw new Exception("Artikl nije pronađen.");

        var client = await _context.Clients.FirstOrDefaultAsync();
        var report = new MedjuskladisnicaReport(client!, article, $"MS-{DateTime.Now:yyyyMMdd}", DateTime.Now, 250m, 360m);
        return report.GeneratePdf();
    }

    // 4. Ulazna kalkulacija
    public async Task<byte[]> GenerateUlaznaKalkulacijaAsync(Guid articleId)
    {
        var article = await _context.Articles.Include(a => a.Warehouse).FirstOrDefaultAsync(a => a.Id == articleId);
        if (article == null) throw new Exception("Artikl nije pronađen.");

        var client = await _context.Clients.FirstOrDefaultAsync();
        var report = new UlaznaKalkulacijaReport(client!, article, $"ULAZ-{DateTime.Now:yyyyMMdd}", DateTime.Now, 250m, 360m);
        return report.GeneratePdf();
    }

    // 5. Otkupni blok s rezervacijom
    public async Task<byte[]> GenerateOtkupniBlokRezervacijaAsync(Guid reservationId)
    {
        var reservation = await _context.Reservations.Include(r => r.Client).FirstOrDefaultAsync(r => r.Id == reservationId);
        if (reservation == null) throw new Exception("Rezervacija nije pronađena.");

        var article = await _context.Articles.FirstOrDefaultAsync();
        var report = new OtkupniBlokRezervacijaReport(reservation.Client!, article!, $"REZ-{DateTime.Now:yyyyMMdd}", DateTime.Now, 50m, 300m);
        return report.GeneratePdf();
    }
}
