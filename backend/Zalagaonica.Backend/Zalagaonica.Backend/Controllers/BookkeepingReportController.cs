using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class BookkeepingReportController : ControllerBase
    {
        private readonly BookkeepingReportService _reportService;
        private readonly BookkeepingPdfService _pdfService;

        public BookkeepingReportController(
            BookkeepingReportService reportService,
            BookkeepingPdfService pdfService)
        {
            _reportService = reportService;
            _pdfService = pdfService;
        }

        [HttpGet("monthly/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyReport(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Mjesec mora biti između 1 i 12");

            var report = await _reportService.GenerateMonthlyReportAsync(year, month);
            return Ok(report);
        }

        [HttpGet("monthly/{year}/{month}/pdf")]
        public async Task<IActionResult> GetMonthlyReportPdf(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Mjesec mora biti između 1 i 12");

            var report = await _reportService.GenerateMonthlyReportAsync(year, month);
            var pdf = _pdfService.GenerateMonthlyReportPdf(report);

            return File(pdf, "application/pdf", $"mjesecni_izvjestaj_{year}_{month:D2}.pdf");
        }

        [HttpGet("kpo")]
        public async Task<IActionResult> GetKpoReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Početni datum mora biti prije krajnjeg datuma");

            var report = await _reportService.GenerateKpoReportAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("kpo/pdf")]
        public async Task<IActionResult> GetKpoReportPdf([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Početni datum mora biti prije krajnjeg datuma");

            var report = await _reportService.GenerateKpoReportAsync(startDate, endDate);
            var pdf = _pdfService.GenerateKpoReportPdf(report);

            return File(pdf, "application/pdf", $"kpo_izvjestaj_{startDate:yyyy_MM_dd}_{endDate:yyyy_MM_dd}.pdf");
        }

        [HttpGet("pledges")]
        public async Task<IActionResult> GetPledgeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Početni datum mora biti prije krajnjeg datuma");

            var report = await _reportService.GeneratePledgeReportAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("pledges/pdf")]
        public async Task<IActionResult> GetPledgeReportPdf([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Početni datum mora biti prije krajnjeg datuma");

            var report = await _reportService.GeneratePledgeReportAsync(startDate, endDate);
            var pdf = _pdfService.GeneratePledgeReportPdf(report);

            return File(pdf, "application/pdf", $"izvjestaj_zaloga_{startDate:yyyy_MM_dd}_{endDate:yyyy_MM_dd}.pdf");
        }

        [HttpGet("tax/{year}/{month}")]
        public async Task<IActionResult> GetTaxReport(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Mjesec mora biti između 1 i 12");

            var report = await _reportService.GenerateTaxReportAsync(year, month);
            return Ok(report);
        }

        [HttpGet("tax/{year}/{month}/pdf")]
        public async Task<IActionResult> GetTaxReportPdf(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Mjesec mora biti između 1 i 12");

            var report = await _reportService.GenerateTaxReportAsync(year, month);
            var pdf = _pdfService.GenerateTaxReportPdf(report);

            return File(pdf, "application/pdf", $"porezni_izvjestaj_{year}_{month:D2}.pdf");
        }
    }
}
