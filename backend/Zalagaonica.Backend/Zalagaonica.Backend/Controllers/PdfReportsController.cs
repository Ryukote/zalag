using Application.DTOs.Reports;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Zalagaonica.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfReportsController : ControllerBase
    {
        private readonly PdfReportsService _pdfReportsService;

        public PdfReportsController(PdfReportsService pdfReportsService)
        {
            _pdfReportsService = pdfReportsService;
        }

        [HttpPost("purchase-receipt")]
        public IActionResult GeneratePurchaseReceipt([FromBody] PurchaseReceiptDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GeneratePurchaseReceipt(data);
                var fileName = $"otkupni-blok-{data.DocumentNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("pledge-agreement")]
        public IActionResult GeneratePledgeAgreement([FromBody] PledgeAgreementDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GeneratePledgeAgreement(data);
                var fileName = $"ugovor-zalog-{data.PledgeNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("inbound-calculation")]
        public IActionResult GenerateInboundCalculation([FromBody] InboundCalculationDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GenerateInboundCalculation(data);
                var fileName = $"ulazna-kalkulacija-{data.DocumentNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("appraisal-request")]
        public IActionResult GenerateAppraisalRequest([FromBody] AppraisalRequestDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GenerateAppraisalRequest(data);
                var fileName = $"zahtjev-procjena-{data.DocumentNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reservation-receipt")]
        public IActionResult GenerateReservationReceipt([FromBody] ReservationReceiptDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GenerateReservationReceipt(data);
                var fileName = $"potvrda-rezervacije-{data.DocumentNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("warehouse-transfer")]
        public IActionResult GenerateWarehouseTransfer([FromBody] WarehouseTransferDto data)
        {
            try
            {
                var pdf = _pdfReportsService.GenerateWarehouseTransfer(data);
                var fileName = $"medjuskladisnica-{data.DocumentNumber}.pdf";
                return File(pdf, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
