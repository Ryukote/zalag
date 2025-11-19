using Application.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("otkupni-blok/{articleId}")]
        public async Task<IActionResult> OtkupniBlok(Guid articleId)
        {
            var pdf = await _reportService.GenerateOtkupniBlokAsync(articleId);
            return File(pdf, "application/pdf", "OtkupniBlok.pdf");
        }

        [HttpGet("zahtjev-procjenu/{articleId}")]
        public async Task<IActionResult> ZahtjevZaProcjenu(Guid articleId)
        {
            var pdf = await _reportService.GenerateZahtjevZaProcjenuAsync(articleId);
            return File(pdf, "application/pdf", "ZahtjevZaProcjenu.pdf");
        }

        [HttpGet("medjuskladisnica/{articleId}")]
        public async Task<IActionResult> Medjuskladisnica(Guid articleId)
        {
            var pdf = await _reportService.GenerateMedjuskladisnicaAsync(articleId);
            return File(pdf, "application/pdf", "Medjuskladisnica.pdf");
        }

        [HttpGet("ulazna-kalkulacija/{articleId}")]
        public async Task<IActionResult> UlaznaKalkulacija(Guid articleId)
        {
            var pdf = await _reportService.GenerateUlaznaKalkulacijaAsync(articleId);
            return File(pdf, "application/pdf", "UlaznaKalkulacija.pdf");
        }

        [HttpGet("otkupni-blok-rezervacija/{reservationId}")]
        public async Task<IActionResult> OtkupniBlokRezervacija(Guid reservationId)
        {
            var pdf = await _reportService.GenerateOtkupniBlokRezervacijaAsync(reservationId);
            return File(pdf, "application/pdf", "OtkupniBlokRezervacija.pdf");
        }
    }
}
