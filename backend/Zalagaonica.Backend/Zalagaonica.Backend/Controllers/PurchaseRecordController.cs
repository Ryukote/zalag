using Application.DTOs.Inventory;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseRecordController : ControllerBase
    {
        private readonly PurchaseRecordService _purchaseRecordService;

        public PurchaseRecordController(PurchaseRecordService purchaseRecordService)
        {
            _purchaseRecordService = purchaseRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchases = await _purchaseRecordService.GetAllAsync();
            return Ok(purchases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var purchase = await _purchaseRecordService.GetByIdAsync(id);

            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var purchase = await _purchaseRecordService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = purchase.Id }, purchase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreatePurchaseRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _purchaseRecordService.UpdateAsync(id, dto);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri ažuriranju zapisa o otkupu", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _purchaseRecordService.DeleteAsync(id);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greška pri brisanju zapisa o otkupu", error = ex.Message });
            }
        }
    }
}
