import {
  createPDF,
  drawCompanyHeader,
  drawTitle,
  drawTextBlock,
  drawSignatureSection,
  formatCurrency,
  formatDate,
  defaultCompanyInfo
} from '../pdfGenerator';

export interface WarehouseTransferItem {
  name: string;
  description: string;
  code: string;
  unitOfMeasure: string;
  quantity: number;
  invoicePrice: number;
  discountPercent: number;
  discountAmount: number;
  purchasePrice: number;
  marginPercent: number;
  marginAmount: number;
  taxPercent: number;
  retailPrice: number;
}

export interface WarehouseTransferData {
  documentNumber: string;
  documentDate: Date;
  fromWarehouse: string;
  toWarehouse: string;
  client?: {
    name: string;
    oib?: string;
    city?: string;
  };
  items: WarehouseTransferItem[];
  totalPurchasePrice: number;
  totalRetailPrice: number;
  note?: string;
  vatInfo?: {
    base: number;
    amount: number;
  };
}

export const generateWarehouseTransfer = (data: WarehouseTransferData): void => {
  const doc = createPDF();
  let y = 15;

  // Warehouse info (top-left, small)
  doc.setFontSize(8);
  doc.setFont('helvetica', 'normal');
  doc.text(`Skladište: ${data.fromWarehouse}`, 15, y);
  y += 15;

  // Company header (left box)
  drawCompanyHeader(doc, defaultCompanyInfo, y);

  // Right-side document info
  const pageWidth = doc.internal.pageSize.getWidth();
  let rightY = y;

  if (data.client) {
    doc.setFontSize(8);
    doc.text(`Podaci o dokumentu na osnovu kojeg nastaje ulazni dokument`, pageWidth - 15, rightY, { align: 'right' });
    rightY += 5;
    doc.text(`Komitent: ${data.client.name}`, pageWidth - 15, rightY, { align: 'right' });
    rightY += 5;
    if (data.client.oib) {
      doc.text(`OIB: ${data.client.oib}`, pageWidth - 15, rightY, { align: 'right' });
      rightY += 5;
    }
    if (data.client.city) {
      doc.text(data.client.city, pageWidth - 15, rightY, { align: 'right' });
      rightY += 5;
    }
    rightY += 3;
  }

  doc.text(`Ulazni dokument: ob ${data.documentNumber.split('-')[0]}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum ul. dokumenta: ${formatDate(data.documentDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum knjiženja: ${formatDate(data.documentDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum dospijeca:`, pageWidth - 15, rightY, { align: 'right' });

  y = Math.max(rightY, 60) + 10;

  // Title
  y = drawTitle(doc, `MEĐUSKLADIŠNJICA: ${data.documentNumber}`, y);
  y += 5;

  // Items table
  const tableData = data.items.map((item, index) => [
    index + 1,
    `${item.name} (${item.description || ''}) ${item.code}`,
    item.unitOfMeasure,
    item.quantity,
    formatCurrency(item.invoicePrice),
    `${item.discountPercent.toFixed(2)} %`,
    formatCurrency(item.discountAmount),
    formatCurrency(item.purchasePrice),
    `${item.marginPercent.toFixed(2)} %`,
    formatCurrency(item.marginAmount),
    `${item.taxPercent.toFixed(0)} %`,
    formatCurrency(item.retailPrice)
  ]);

  // Add totals row
  const isNegative = data.totalRetailPrice < 0;
  tableData.push([
    '',
    '',
    '',
    '',
    `Ukupno (kn):`,
    formatCurrency(Math.abs(data.totalPurchasePrice)),
    '',
    formatCurrency(isNegative ? -Math.abs(data.totalPurchasePrice) : Math.abs(data.totalPurchasePrice)),
    formatCurrency(Math.abs(data.totalRetailPrice - data.totalPurchasePrice)),
    '',
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
      'faktura c\nfaktura vr.',
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
      1: { cellWidth: 50 },
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

  // Note if provided
  if (data.note) {
    doc.setFontSize(8);
    doc.setFont('helvetica', 'italic');
    doc.text(`Veza na dok.`, 15, y);
    y += 10;
  }

  // VAT summary
  doc.setFontSize(8);
  doc.setFont('helvetica', 'bold');
  doc.text('Izradio: Tin Matija Bertić', 15, y);
  doc.text('m.p.', 15, y + 5);

  if (data.vatInfo) {
    const vatTableX = 80;
    doc.text('porez %', vatTableX, y);
    doc.setFont('helvetica', 'normal');

    doc.autoTable({
      startY: y + 3,
      head: [['osnovica', 'iznos']],
      body: [
        [formatCurrency(data.vatInfo.base), formatCurrency(data.vatInfo.amount)]
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
    doc.text(formatCurrency(data.vatInfo.base), pageWidth - 40, finalY, { align: 'right' });
    doc.text(formatCurrency(data.vatInfo.amount), pageWidth - 15, finalY, { align: 'right' });

    // Porezna naknada
    doc.text('Porezna naknada : 0,00', pageWidth - 15, finalY + 10, { align: 'right' });
  }

  // Add page number
  doc.setFontSize(7);
  doc.setFont('helvetica', 'italic');
  const pageHeight = doc.internal.pageSize.getHeight();
  doc.text('1 od 1', pageWidth - 15, pageHeight - 10, { align: 'right' });

  // Save/download the PDF
  doc.save(`meduskladisnjica-${data.documentNumber}.pdf`);
};
