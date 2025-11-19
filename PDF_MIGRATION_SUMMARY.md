# PDF Generation Migration - Summary

## Overview
All PDF generation has been successfully migrated from frontend (React-PDF/jsPDF) to backend (QuestPDF in C#/.NET).

## What Was Changed

### Backend Changes

#### 1. Created DTOs (Data Transfer Objects)
Location: `backend/Zalagaonica.Backend/Application/DTOs/Reports/`

- **PurchaseReceiptDto.cs** - For purchase/otkup receipts
- **PledgeAgreementDto.cs** - For pledge/zalog agreements
- **InboundCalculationDto.cs** - For inbound calculations/ulazna kalkulacija
- **AppraisalRequestDto.cs** - For appraisal requests/zahtjev za procjenu
- **ReservationReceiptDto.cs** - For reservation receipts/potvrda rezervacije
- **WarehouseTransferDto.cs** - For warehouse transfers/međuskladišnica

#### 2. Created QuestPDF Report Templates
Location: `backend/Zalagaonica.Backend/Application/Reports/Templates/`

- **PurchaseReceiptReport.cs** - Generates purchase receipt PDF
- **PledgeAgreementReport.cs** - Generates pledge agreement PDF
- **InboundCalculationReport.cs** - Generates inbound calculation PDF
- **AppraisalRequestReport.cs** - Generates appraisal request PDF
- **ReservationReceiptReport.cs** - Generates reservation receipt PDF
- **WarehouseTransferReport.cs** - Generates warehouse transfer PDF

All templates support Croatian characters and follow the same design as the frontend versions.

#### 3. Created PDF Reports Service
Location: `backend/Zalagaonica.Backend/Application/Services/PdfReportsService.cs`

Service that orchestrates PDF generation using QuestPDF templates.

#### 4. Created PDF Reports Controller
Location: `backend/Zalagaonica.Backend/Zalagaonica.Backend/Controllers/PdfReportsController.cs`

REST API endpoints:
- `POST /api/PdfReports/purchase-receipt` - Generate purchase receipt
- `POST /api/PdfReports/pledge-agreement` - Generate pledge agreement
- `POST /api/PdfReports/inbound-calculation` - Generate inbound calculation
- `POST /api/PdfReports/appraisal-request` - Generate appraisal request
- `POST /api/PdfReports/reservation-receipt` - Generate reservation receipt
- `POST /api/PdfReports/warehouse-transfer` - Generate warehouse transfer

#### 5. Registered Service in Program.cs
Added `builder.Services.AddScoped<PdfReportsService>();` to dependency injection.

### Frontend Changes

#### 1. Created API Service
Location: `zalagaonica-web/src/services/pdfReportsApi.ts`

TypeScript service that calls backend API endpoints and handles PDF downloads.

#### 2. Updated Pages
- **ClientPage.tsx** - Now calls backend for purchase receipts and pledge agreements
- **PledgeListPage.tsx** - Now calls backend for pledge agreement regeneration
- **IncomingDocumentsPage.tsx** - Now calls backend for inbound calculations

All frontend pages now use the new `pdfReportsApi` service instead of generating PDFs locally.

## Benefits of This Migration

1. **Better Performance** - PDF generation happens on the server, not blocking the UI
2. **Consistency** - All PDFs generated using the same backend library (QuestPDF)
3. **Maintainability** - Single source of truth for PDF templates
4. **Scalability** - Backend can handle multiple PDF generation requests
5. **Security** - Business logic and PDF generation on the server
6. **Professional Output** - QuestPDF produces high-quality, professional PDFs

## Configuration Required

### Backend
No additional configuration needed. QuestPDF is already included in the project.

### Frontend
Ensure the API URL is correctly configured:
- Default: `http://localhost:5000/api`
- Set `REACT_APP_API_URL` environment variable if using a different URL

Example `.env` file:
```
REACT_APP_API_URL=http://localhost:5000/api
```

## Testing Instructions

### 1. Start Backend
```bash
cd backend/Zalagaonica.Backend/Zalagaonica.Backend
dotnet run
```

### 2. Start Frontend
```bash
cd zalagaonica-web
npm start
```

### 3. Test Purchase Receipt
1. Navigate to Clients page (Komitenti)
2. Click "Novi otkup" (New purchase) for any client
3. Fill in item details and purchase amount
4. Complete the process
5. PDF should download automatically

### 4. Test Pledge Agreement
1. Navigate to Clients page (Komitenti)
2. Click "Novi zalog" (New pledge) for any client
3. Fill in item details and loan amount
4. Complete the process
5. PDF should download automatically

### 5. Test Pledge Agreement Regeneration
1. Navigate to Pledge List page (Knjiga Zaloga)
2. Click the document icon for any pledge
3. PDF should download automatically

### 6. Test Inbound Calculation
1. Navigate to Incoming Documents page (Ulazni dokumenti)
2. Click the generate report button for any document
3. PDF should download automatically

## Troubleshooting

### PDF Not Downloading
- Check browser console for errors
- Verify backend is running (`http://localhost:5000`)
- Check CORS settings in backend Program.cs
- Verify API_BASE_URL in frontend

### Backend Errors
- Check backend console for error messages
- Verify all DTOs are correctly populated
- Check QuestPDF dependencies are installed

### Croatian Characters Not Displaying
- QuestPDF templates are configured to use standard fonts
- Should display Croatian characters correctly
- If issues persist, check font configuration in report templates

## API Endpoint Examples

### Generate Purchase Receipt
```bash
POST http://localhost:5000/api/PdfReports/purchase-receipt
Content-Type: application/json

{
  "documentNumber": "OTK-001",
  "documentDate": "2025-01-17T00:00:00",
  "seller": {
    "name": "John Doe",
    "address": "Main Street 123",
    "city": "Zagreb",
    "oib": "12345678901"
  },
  "items": [{
    "name": "Gold Ring",
    "code": "RING-001",
    "quantity": 1,
    "unitOfMeasure": "KOM",
    "mpc": 500.00,
    "purchasePrice": 300.00
  }],
  "warehouse": "Glavno skladište",
  "employeeName": "Admin"
}
```

### Generate Pledge Agreement
```bash
POST http://localhost:5000/api/PdfReports/pledge-agreement
Content-Type: application/json

{
  "pledgeNumber": "ZALOG-001",
  "pledgeDate": "2025-01-17T00:00:00",
  "client": {
    "name": "John Doe",
    "address": "Main Street 123",
    "city": "Zagreb",
    "oib": "12345678901"
  },
  "item": {
    "name": "Gold Ring",
    "description": "18K gold ring with diamond",
    "estimatedValue": 500.00
  },
  "loanAmount": 300.00,
  "returnAmount": 330.00,
  "period": 30,
  "redeemDeadline": "2025-02-16T00:00:00"
}
```

## Next Steps

### Optional Enhancements
1. **Add Authentication** - Protect API endpoints with JWT authentication
2. **Add Logging** - Log PDF generation requests for audit trail
3. **Add Caching** - Cache generated PDFs for frequently accessed documents
4. **Add Email Integration** - Send PDFs via email directly from backend
5. **Add PDF Preview** - Return PDF as base64 for in-browser preview before download

### Cleanup (Optional)
The following frontend files are no longer used and can be removed:
- `src/utils/reports/purchaseReceiptReactPDF.tsx`
- `src/utils/reports/pledgeAgreementReactPDF.tsx`
- `src/utils/reports/inboundCalculationReactPDF.tsx`
- `src/utils/reports/appraisalRequestReactPDF.tsx`
- `src/utils/reports/reservationReceiptReactPDF.tsx`
- `src/utils/reports/warehouseTransferReactPDF.tsx`
- All old jsPDF-based report files

**Note:** Keep these files temporarily until you've verified all functionality works correctly in production.

## Support

If you encounter any issues:
1. Check backend logs for error messages
2. Check browser console for frontend errors
3. Verify API endpoint URLs are correct
4. Test API endpoints directly using Postman or curl
5. Check CORS configuration if getting CORS errors

## Conclusion

The PDF generation migration is complete! All PDFs are now generated on the backend using QuestPDF, providing better performance, consistency, and maintainability. The frontend simply calls the backend API endpoints and handles the PDF download.
