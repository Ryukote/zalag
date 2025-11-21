using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _service;

        public AnalyticsController(AnalyticsService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _service.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju statistike", error = ex.Message });
            }
        }

        [HttpGet("sales-chart")]
        public async Task<IActionResult> GetSalesChartData([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
                var end = endDate ?? DateTime.UtcNow;

                var data = await _service.GetSalesChartDataAsync(start, end);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju podataka o prodaji", error = ex.Message });
            }
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int count = 10)
        {
            try
            {
                var products = await _service.GetTopSellingProductsAsync(count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju najprodavanijih proizvoda", error = ex.Message });
            }
        }

        [HttpGet("warehouse-stats")]
        public async Task<IActionResult> GetWarehouseStats()
        {
            try
            {
                var stats = await _service.GetWarehouseStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju statistike skladišta", error = ex.Message });
            }
        }

        [HttpGet("pledge-stats")]
        public async Task<IActionResult> GetPledgeStats()
        {
            try
            {
                var stats = await _service.GetPledgeStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju statistike zaloga", error = ex.Message });
            }
        }

        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int months = 12)
        {
            try
            {
                var revenue = await _service.GetMonthlyRevenueAsync(months);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju mjesečnih prihoda", error = ex.Message });
            }
        }

        [HttpGet("top-clients")]
        public async Task<IActionResult> GetTopClients([FromQuery] int count = 10)
        {
            try
            {
                var clients = await _service.GetTopClientsAsync(count);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju najvažnijih klijenata", error = ex.Message });
            }
        }
    }
}
