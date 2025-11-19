using Application.DTOs.Reports;
using Application.Reports.Templates;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfController : ControllerBase
    {
        [HttpPost("pledge-agreement")]
        public IActionResult GeneratePledgeAgreement([FromBody] PledgeAgreementPdfRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = new UgovorOZaloguReport.PledgeAgreementData
            {
                PledgeNumber = request.PledgeNumber,
                PledgeDate = request.PledgeDate,
                ClientName = request.ClientName,
                ClientAddress = request.ClientAddress,
                ClientCity = request.ClientCity,
                ClientOib = request.ClientOib,
                ItemName = request.ItemName,
                ItemDescription = request.ItemDescription,
                EstimatedValue = request.EstimatedValue,
                LoanAmount = request.LoanAmount,
                ReturnAmount = request.ReturnAmount,
                Period = request.Period,
                RedeemDeadline = request.RedeemDeadline
            };

            var document = new UgovorOZaloguReport(data);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Ugovor-O-Zalogu-{request.PledgeNumber}.pdf");
        }

        [HttpPost("purchase-receipt")]
        public IActionResult GeneratePurchaseReceipt([FromBody] PurchaseReceiptPdfRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create temporary entities from DTO data
            var client = new Client
            {
                Name = request.ClientName,
                Address = request.ClientAddress
            };

            var article = new Article
            {
                Name = request.ArticleName,
                Description = request.ArticleDescription
            };

            var document = new OtkupniBlokReport(
                client,
                article,
                request.Price,
                request.DocumentNumber,
                request.Date
            );

            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Otkupni-Blok-{request.DocumentNumber}.pdf");
        }

        [HttpPost("payment-receipt")]
        public IActionResult GeneratePaymentReceipt([FromBody] PaymentReceiptPdfRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Name = request.ClientName,
                Address = request.ClientAddress,
                PhoneNumber = request.ClientPhoneNumber
            };

            var article = new Article
            {
                Id = request.ArticleId,
                Name = request.ArticleName
            };

            var document = new RacunOIsplatiReport(
                client,
                article,
                request.DocumentNumber,
                request.Date,
                request.Amount
            );

            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Racun-O-Isplati-{request.DocumentNumber}.pdf");
        }

        [HttpPost("appraisal-request")]
        public IActionResult GenerateAppraisalRequest([FromBody] AppraisalRequestPdfRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Name = request.ClientName,
                Address = request.ClientAddress,
                PhoneNumber = request.ClientPhoneNumber
            };

            var article = new Article
            {
                Name = request.ItemName,
                Description = request.ItemDescription
            };

            var document = new ZahtjevZaProcjenuReport(
                client,
                article,
                request.ItemDescription ?? "",
                $"ZP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                request.RequestDate
            );

            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Zahtjev-Za-Procjenu-{request.RequestDate:yyyyMMdd}.pdf");
        }
    }
}
