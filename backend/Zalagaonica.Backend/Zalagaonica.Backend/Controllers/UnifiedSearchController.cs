using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [Authorize] // Accessible by both Admin and Worker
    [ApiController]
    [Route("api/[controller]")]
    public class UnifiedSearchController : ControllerBase
    {
        private readonly UnifiedDocumentSearchService _service;

        public UnifiedSearchController(UnifiedDocumentSearchService service)
        {
            _service = service;
        }

        /// <summary>
        /// Advanced search across all document types
        /// </summary>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] DocumentSearchQuery query)
        {
            try
            {
                var results = await _service.SearchDocumentsAsync(query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri pretraživanju dokumenata", error = ex.Message });
            }
        }

        /// <summary>
        /// Quick search for autocomplete - searches clients, articles, and documents
        /// </summary>
        [HttpGet("quick")]
        public async Task<IActionResult> QuickSearch([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                {
                    return Ok(new List<QuickSearchResultDto>());
                }

                var results = await _service.QuickSearchAsync(q);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri brzom pretraživanju", error = ex.Message });
            }
        }

        /// <summary>
        /// Get search statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetSearchStats()
        {
            try
            {
                var query = new DocumentSearchQuery
                {
                    DateFrom = DateTime.UtcNow.AddMonths(-1),
                    DateTo = DateTime.UtcNow
                };

                var results = await _service.SearchDocumentsAsync(query);

                var stats = new
                {
                    lastMonthSales = results.Sales.Count,
                    lastMonthPurchases = results.Purchases.Count,
                    lastMonthPledges = results.Pledges.Count,
                    lastMonthPurchaseRecords = results.PurchaseRecords.Count,
                    lastMonthOutputDocuments = results.OutputDocuments.Count,
                    total = results.TotalResults
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri dohvaćanju statistike", error = ex.Message });
            }
        }
    }
}
