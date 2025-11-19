using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendVacationRequestNotificationAsync(string employeeName, DateTime startDate, DateTime endDate, int totalDays, string? reason)
        {
            try
            {
                var adminEmail = _configuration["Email:AdminEmail"] ?? "admin@pawnshop.hr";
                var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@pawnshop.hr";
                var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUser = _configuration["Email:SmtpUser"] ?? "";
                var smtpPass = _configuration["Email:SmtpPassword"] ?? "";

                var subject = $"Nova zahtjev za godišnji odmor - {employeeName}";
                var body = $@"
                    <html>
                    <body>
                        <h2>Novi zahtjev za godišnji odmor</h2>
                        <p><strong>Zaposlenik:</strong> {employeeName}</p>
                        <p><strong>Datum početka:</strong> {startDate:dd.MM.yyyy}</p>
                        <p><strong>Datum kraja:</strong> {endDate:dd.MM.yyyy}</p>
                        <p><strong>Ukupno dana:</strong> {totalDays}</p>
                        {(string.IsNullOrEmpty(reason) ? "" : $"<p><strong>Razlog:</strong> {reason}</p>")}
                        <p>Molimo prijavite se u sustav za odobrenje ili odbijanje zahtjeva.</p>
                    </body>
                    </html>
                ";

                using var message = new MailMessage(fromEmail, adminEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, smtpPass)
                };

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Vacation request notification sent to {adminEmail} for employee {employeeName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send vacation request notification email");
                // Don't throw - email failure shouldn't block the vacation request
            }
        }

        public async Task SendVacationStatusUpdateAsync(string employeeName, string employeeEmail, DateTime startDate, DateTime endDate, bool approved, string? rejectionReason = null)
        {
            try
            {
                var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@pawnshop.hr";
                var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUser = _configuration["Email:SmtpUser"] ?? "";
                var smtpPass = _configuration["Email:SmtpPassword"] ?? "";

                var subject = approved
                    ? "Godišnji odmor odobren"
                    : "Godišnji odmor odbijen";

                var body = approved
                    ? $@"
                        <html>
                        <body>
                            <h2>Vaš zahtjev za godišnji odmor je odobren</h2>
                            <p>Poštovani/a {employeeName},</p>
                            <p>Vaš zahtjev za godišnji odmor je odobren.</p>
                            <p><strong>Datum početka:</strong> {startDate:dd.MM.yyyy}</p>
                            <p><strong>Datum kraja:</strong> {endDate:dd.MM.yyyy}</p>
                            <p>Ugodan odmor!</p>
                        </body>
                        </html>
                    "
                    : $@"
                        <html>
                        <body>
                            <h2>Vaš zahtjev za godišnji odmor je odbijen</h2>
                            <p>Poštovani/a {employeeName},</p>
                            <p>Nažalost, vaš zahtjev za godišnji odmor je odbijen.</p>
                            <p><strong>Datum početka:</strong> {startDate:dd.MM.yyyy}</p>
                            <p><strong>Datum kraja:</strong> {endDate:dd.MM.yyyy}</p>
                            {(string.IsNullOrEmpty(rejectionReason) ? "" : $"<p><strong>Razlog:</strong> {rejectionReason}</p>")}
                            <p>Za dodatna pitanja, molimo kontaktirajte administratora.</p>
                        </body>
                        </html>
                    ";

                using var message = new MailMessage(fromEmail, employeeEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, smtpPass)
                };

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Vacation status update sent to {employeeEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send vacation status update email");
            }
        }
    }
}
