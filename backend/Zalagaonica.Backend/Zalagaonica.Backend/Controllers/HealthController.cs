using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HealthController> _logger;

        public HealthController(ApplicationDbContext dbContext, ILogger<HealthController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Health check endpoint for Docker and monitoring systems
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Check database connectivity
                var canConnect = await _dbContext.Database.CanConnectAsync();

                if (!canConnect)
                {
                    _logger.LogWarning("Health check failed: Cannot connect to database");
                    return StatusCode(503, new
                    {
                        status = "Unhealthy",
                        message = "Database connection failed",
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    status = "Healthy",
                    message = "Service is running",
                    database = "Connected",
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed with exception");
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Readiness check - more detailed than health check
        /// </summary>
        [HttpGet("ready")]
        public async Task<IActionResult> Ready()
        {
            try
            {
                var canConnect = await _dbContext.Database.CanConnectAsync();

                // Check if migrations are applied
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                var hasPendingMigrations = pendingMigrations.Any();

                if (!canConnect || hasPendingMigrations)
                {
                    return StatusCode(503, new
                    {
                        status = "Not Ready",
                        database_connected = canConnect,
                        pending_migrations = hasPendingMigrations,
                        timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new
                {
                    status = "Ready",
                    database_connected = true,
                    pending_migrations = false,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Readiness check failed");
                return StatusCode(503, new
                {
                    status = "Not Ready",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Liveness check - simple check if the service is alive
        /// </summary>
        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                status = "Alive",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
