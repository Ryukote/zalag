using Application.DTOs.Inventory;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PledgeController : ControllerBase
    {
        private readonly PledgeService _pledgeService;

        public PledgeController(PledgeService pledgeService)
        {
            _pledgeService = pledgeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pledges = await _pledgeService.GetAllAsync();
            return Ok(pledges);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var pledge = await _pledgeService.GetByIdAsync(id);

            if (pledge == null)
                return NotFound();

            return Ok(pledge);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePledgeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pledge = await _pledgeService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = pledge.Id }, pledge);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePledgeDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _pledgeService.UpdateAsync(dto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/redeem")]
        public async Task<IActionResult> Redeem(Guid id)
        {
            var success = await _pledgeService.RedeemAsync(id);

            if (!success)
                return BadRequest(new { message = "Ne može se otkupiti zalog - zalog ne postoji ili je već otkupljen/preuzet" });

            return Ok(new { message = "Zalog uspješno otkupljen" });
        }

        [HttpPost("{id}/forfeit")]
        public async Task<IActionResult> Forfeit(Guid id)
        {
            var success = await _pledgeService.ForfeitAsync(id);

            if (!success)
                return BadRequest(new { message = "Ne može se preuzeti zalog - zalog ne postoji ili je već otkupljen/preuzet" });

            return Ok(new { message = "Zalog uspješno preuzet i prebačen u prodaju" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _pledgeService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
