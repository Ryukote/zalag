import {
  createPDF,
  drawCompanyHeader,
  drawClientInfo,
  drawDocumentInfo,
  drawTitle,
  drawTextBlock,
  drawSignatureSection,
  formatCurrency,
  formatDate,
  defaultCompanyInfo
} from '../pdfGenerator';

export interface InboundCalculationItem {
  name: string;
  description: string;
  quantity: number;
  unitOfMeasure: string;
  invoicePrice: number;
  discountPercent: number;
  discountAmount: number;
  purchasePrice: number;
  marginPercent: number;
  marginAmount: number;
  taxPercent: number;
  taxAmount: number;
  retailPrice: number;
}

export interface InboundCalculationData {
  documentNumber: string;
  documentDate: Date;
  incomingDocumentNumber: string;
  incomingDocumentDate: Date;
  seller: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  warehouse: string;
  items: InboundCalculationItem[];
  totalInvoicePrice: number;
  totalPurchasePrice: number;
  totalMargin: number;
  totalTax: number;
  totalRetailPrice: number;
  vatOnAddedValue: {
    base: number;
    amount: number;
  };
}

export const generateInboundCalculation = (data: InboundCalculationData): void => {
  const doc = createPDF();
  let y = 15;

  // Warehouse/location info (top-left, small)
  doc.setFontSize(8);
  doc.setFont('helvetica', 'normal');
  doc.text(`Skladište: ${data.warehouse}`, 15, y);
  y += 15;

  // Company header (left box)
  drawCompanyHeader(doc, defaultCompanyInfo, y);

  // Right-side document info
  const pageWidth = doc.internal.pageSize.getWidth();
  let rightY = y;
  doc.setFontSize(8);
  doc.text(`Podaci o dokumentu na osnovu kojeg nastaje ulazni dokument`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Komitent: ${data.seller.name}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  if (data.seller.oib) {
    doc.text(`OIB: ${data.seller.oib}`, pageWidth - 15, rightY, { align: 'right' });
    rightY += 5;
  }
  if (data.seller.city) {
    doc.text(data.seller.city, pageWidth - 15, rightY, { align: 'right' });
    rightY += 5;
  }
  rightY += 3;
  doc.text(`Ulazni dokument: od ${data.incomingDocumentNumber}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum ul. dokumenta: ${formatDate(data.incomingDocumentDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum knjiženja: ${formatDate(data.documentDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum dospijeca:`, pageWidth - 15, rightY, { align: 'right' });

  y = Math.max(rightY, 60) + 10;

  // Title
  y = drawTitle(doc, `ULAZNA KALKULACIJA: ${data.documentNumber}`, y);
  y += 5;

  // Items table
  const tableData = data.items.map((item, index) => [
    index + 1,
    `${item.name} (${item.description})`,
    item.unitOfMeasure,
    item.quantity,
    formatCurrency(item.invoicePrice),
    `${item.discountPercent.toFixed(2)} %`,
    formatCurrency(item.discountAmount),
    formatCurrency(item.purchasePrice),
    `${item.marginPercent.toFixed(2)} %`,
    formatCurrency(item.marginAmount),
    `${item.taxPercent.toFixed(0)} %`,
    formatCurrency(item.taxAmount),
    formatCurrency(item.retailPrice)
  ]);

  // Add totals row
  tableData.push([
    '',
    '',
    '',
    '',
    '',
    '',
    '',
    `Ukupno (kn):`,
    formatCurrency(data.totalPurchasePrice),
    '',
    formatCurrency(data.totalPurchasePrice),
    formatCurrency(data.totalMargin),
    '',
    formatCurrency(data.totalRetailPrice)
  ]);

  doc.autoTable({
    startY: y,
    head: [[
      '',
      'naziv artikla\noznaka / šifra\nkod artikla',
      'jed. mjere',
      'količina',
      'faktura c\nfaktura v',
      'rabat (%)\nrabat vr.',
      'nabavna c\nnabavna vr.',
      'marža (%)\nmarža uk.',
      'porez %',
      'mpc (eur)\nmpv (eur)'
    ]],
    body: tableData,
    theme: 'grid',
    styles: {
      fontSize: 7,
      cellPadding: 1.5
    },
    headStyles: {
      fillColor: [255, 255, 255],
      textColor: [0, 0, 0],
      fontStyle: 'bold',
      lineWidth: 0.1,
      lineColor: [0, 0, 0],
      minCellHeight: 10
    },
    columnStyles: {
      0: { halign: 'center', cellWidth: 8 },
      1: { cellWidth: 45 },
      2: { halign: 'center', cellWidth: 12 },
      3: { halign: 'center', cellWidth: 12 },
      4: { halign: 'right', cellWidth: 18 },
      5: { halign: 'right', cellWidth: 18 },
      6: { halign: 'right', cellWidth: 18 },
      7: { halign: 'right', cellWidth: 18 },
      8: { halign: 'right', cellWidth: 15 },
      9: { halign: 'right', cellWidth: 18 }
    }
  });

  y = doc.lastAutoTable.finalY + 10;

  // VAT summary table
  doc.setFontSize(8);
  doc.setFont('helvetica', 'bold');
  doc.text('Izradio: Tin Matija Bertić', 15, y);
  doc.text('m.p.', 15, y + 5);

  // Porez % table
  const vatTableX = 80;
  doc.text('porez %', vatTableX, y);

  doc.autoTable({
    startY: y + 3,
    head: [['osnovica', 'iznos']],
    body: [
      [formatCurrency(data.vatOnAddedValue.base), formatCurrency(data.vatOnAddedValue.amount)]
    ],
    theme: 'grid',
    styles: {
      fontSize: 8,
      cellPadding: 2
    },
    margin: { left: vatTableX },
    tableWidth: 60,
    headStyles: {
      fillColor: [255, 255, 255],
      textColor: [0, 0, 0],
      fontStyle: 'bold'
    }
  });

  const finalY = doc.lastAutoTable.finalY;

  // Bottom right: VAT on added value
  doc.setFontSize(8);
  doc.text('POREZ na dodanu vrijednost', pageWidth - 80, finalY);
  doc.text(formatCurrency(data.vatOnAddedValue.base), pageWidth - 40, finalY, { align: 'right' });
  doc.text(formatCurrency(data.vatOnAddedValue.amount), pageWidth - 15, finalY, { align: 'right' });

  // Porezna naknada
  doc.text('Porezna naknada : 0,00', pageWidth - 15, finalY + 10, { align: 'right' });

  // Add page number
  doc.setFontSize(7);
  doc.setFont('helvetica', 'italic');
  doc.text('1 od 1', pageWidth - 15, doc.internal.pageSize.getHeight() - 10, { align: 'right' });

  // Save/download the PDF
  doc.save(`ulazna-kalkulacija-${data.documentNumber}.pdf`);
};
