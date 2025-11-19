using Application.DTOs.Reports;
using Application.Reports.Templates;
using QuestPDF.Fluent;

namespace Application.Services
{
    public class PdfReportsService
    {
        public byte[] GeneratePurchaseReceipt(PurchaseReceiptDto data)
        {
            var report = new PurchaseReceiptReport(data);
            return report.GeneratePdf();
        }

        public byte[] GeneratePledgeAgreement(PledgeAgreementDto data)
        {
            var report = new PledgeAgreementReport(data);
            return report.GeneratePdf();
        }

        public byte[] GenerateInboundCalculation(InboundCalculationDto data)
        {
            var report = new InboundCalculationReport(data);
            return report.GeneratePdf();
        }

        public byte[] GenerateAppraisalRequest(AppraisalRequestDto data)
        {
            var report = new AppraisalRequestReport(data);
            return report.GeneratePdf();
        }

        public byte[] GenerateReservationReceipt(ReservationReceiptDto data)
        {
            var report = new ReservationReceiptReport(data);
            return report.GeneratePdf();
        }

        public byte[] GenerateWarehouseTransfer(WarehouseTransferDto data)
        {
            var report = new WarehouseTransferReport(data);
            return report.GeneratePdf();
        }
    }
}
