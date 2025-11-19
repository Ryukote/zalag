using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VacationController : ControllerBase
    {
        private readonly VacationService _service;
        private readonly ApplicationDbContext _context;

        public VacationController(VacationService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // Get all vacations (admin only)
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            var vacations = await _service.GetAllAsync();
            return Ok(vacations.Select(v => new VacationResponseDto
            {
                Id = v.Id,
                EmployeeId = v.EmployeeId,
                EmployeeName = v.EmployeeName,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Status = v.Status,
                Type = v.Type,
                RequestDate = v.RequestDate,
                Reason = v.Reason,
                ApprovedBy = v.ApprovedBy,
                ApprovedDate = v.ApprovedDate,
                RejectionReason = v.RejectionReason,
                TotalDays = v.TotalDays
            }));
        }

        // Get pending vacation requests (admin only)
        [HttpGet("pending")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetPending()
        {
            var vacations = await _service.GetPendingAsync();
            return Ok(vacations.Select(v => new VacationResponseDto
            {
                Id = v.Id,
                EmployeeId = v.EmployeeId,
                EmployeeName = v.EmployeeName,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Status = v.Status,
                Type = v.Type,
                RequestDate = v.RequestDate,
                Reason = v.Reason,
                TotalDays = v.TotalDays
            }));
        }

        // Get my vacation requests
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            // Get employee for this user
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
                return Unauthorized("Email claim not found");

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (employee == null)
                return NotFound("Employee not found");

            var vacations = await _service.GetByEmployeeIdAsync(employee.Id);
            return Ok(vacations.Select(v => new VacationResponseDto
            {
                Id = v.Id,
                EmployeeId = v.EmployeeId,
                EmployeeName = v.EmployeeName,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Status = v.Status,
                Type = v.Type,
                RequestDate = v.RequestDate,
                Reason = v.Reason,
                ApprovedBy = v.ApprovedBy,
                ApprovedDate = v.ApprovedDate,
                RejectionReason = v.RejectionReason,
                TotalDays = v.TotalDays
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            return Ok(new VacationResponseDto
            {
                Id = entity.Id,
                EmployeeId = entity.EmployeeId,
                EmployeeName = entity.EmployeeName,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                Type = entity.Type,
                RequestDate = entity.RequestDate,
                Reason = entity.Reason,
                ApprovedBy = entity.ApprovedBy,
                ApprovedDate = entity.ApprovedDate,
                RejectionReason = entity.RejectionReason,
                TotalDays = entity.TotalDays
            });
        }

        // Request vacation
        [HttpPost("request")]
        public async Task<IActionResult> RequestVacation([FromBody] VacationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            // Get employee for this user
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (employee == null)
                return NotFound("Employee not found");

            var vacation = new Vacation
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.FullName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Type = request.Type,
                Reason = request.Reason
            };

            var created = await _service.CreateAsync(vacation);

            return Ok(new VacationResponseDto
            {
                Id = created.Id,
                EmployeeId = created.EmployeeId,
                EmployeeName = created.EmployeeName,
                StartDate = created.StartDate,
                EndDate = created.EndDate,
                Status = created.Status,
                Type = created.Type,
                RequestDate = created.RequestDate,
                Reason = created.Reason,
                TotalDays = created.TotalDays
            });
        }

        // Approve vacation (admin only)
        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var success = await _service.ApproveAsync(id, userId.Value);
            return success ? Ok(new { message = "Vacation approved" }) : NotFound();
        }

        // Reject vacation (admin only)
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] VacationApprovalDto approval)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var success = await _service.RejectAsync(id, userId.Value, approval.RejectionReason);
            return success ? Ok(new { message = "Vacation rejected" }) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Vacation model)
        {
            if (id != model.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _service.UpdateAsync(model);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}
