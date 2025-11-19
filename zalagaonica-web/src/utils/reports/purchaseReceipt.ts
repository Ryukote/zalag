import {
  createPDF,
  drawCompanyHeader,
  drawClientInfo,
  drawTitle,
  drawTextBlock,
  formatCurrency,
  formatDate,
  defaultCompanyInfo
} from '../pdfGenerator';

export interface PurchaseReceiptData {
  documentNumber: string;
  documentDate: Date;
  seller: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  items: Array<{
    name: string;
    code: string; // oznaka / šifra
    quantity: number;
    unitOfMeasure: string;
    mpc: number; // Retail price
    purchasePrice: number; // MPV (eur) - Purchase price
  }>;
  warehouse: string;
  employeeName: string; // Person who created the document
}

export const generatePurchaseReceipt = (data: PurchaseReceiptData): void => {
  const doc = createPDF();
  let y = 15;

  // Company header (left) - KUPAC
  y = drawCompanyHeader(doc, defaultCompanyInfo, y);
  y += 5;

  // Seller info (right) - PRODAVATELJ
  drawClientInfo(doc, data.seller, 15, 'Prodavatelj:');

  // Document metadata (below company header)
  const pageWidth = doc.internal.pageSize.getWidth();
  doc.setFontSize(8);
  doc.text(`Datum dokumenta: ${formatDate(data.documentDate)}`, 15, y);
  y += 5;
  doc.text(`Skladište: ${data.warehouse}`, 15, y);

  y = Math.max(y, 55) + 10;

  // Title
  y = drawTitle(doc, `OTKUPNI BLOK ZA OTKUP RABLJENOG DOBRA: ${data.documentNumber}`, y);
  y += 5;

  // Items table
  const tableData = data.items.map((item, index) => [
    index + 1,
    item.name,
    item.code,
    item.unitOfMeasure,
    item.quantity,
    formatCurrency(item.mpc),
    formatCurrency(item.purchasePrice)
  ]);

  doc.autoTable({
    startY: y,
    head: [['', 'naziv artikla', 'oznaka / šifra', 'jed. mjere', 'količina', 'MPC', 'MPV (eur)']],
    body: tableData,
    theme: 'grid',
    styles: {
      fontSize: 8,
      cellPadding: 2
    },
    headStyles: {
      fillColor: [255, 255, 255],
      textColor: [0, 0, 0],
      fontStyle: 'bold',
      lineWidth: 0.1,
      lineColor: [0, 0, 0]
    },
    columnStyles: {
      0: { halign: 'center', cellWidth: 10 },
      1: { cellWidth: 60 },
      2: { cellWidth: 30 },
      3: { halign: 'center', cellWidth: 20 },
      4: { halign: 'center', cellWidth: 20 },
      5: { halign: 'right', cellWidth: 25 },
      6: { halign: 'right', cellWidth: 25 }
    }
  });

  y = doc.lastAutoTable.finalY + 10;

  // Calculate total
  const totalAmount = data.items.reduce((sum, item) => sum + (item.purchasePrice * item.quantity), 0);

  // Total amount
  doc.setFontSize(10);
  doc.setFont('courier', 'bold');
  doc.text(`Ukupno: ${formatCurrency(totalAmount)}`, pageWidth - 15, y, { align: 'right' });
  y += 10;

  // Legal statement from seller
  doc.setFontSize(8);
  doc.setFont('courier', 'normal');
  doc.text('Izjava prodavatelja:', 15, y);
  y += 5;

  const sellerStatement = `Izjavljujem da je predmet isključivo moje vlasništvo. Suglasan sam da: PAWN SHOPS d.o.o. ne odgovara za dokumente, podatke, informacije, autorska i vlasnička prava trećih osoba te bilo koji drugi sadržaj, kao ni moguću štetu prema prodavatelju ili bilo kojoj trećoj strani koja bi mogla nastati zbog upotrebe istih.

Izjavljujem da nisam porezni obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, niti sam obveznik fiskalizacije.`;

  y = drawTextBlock(doc, sellerStatement, 15, y, 180);
  y += 10;

  // Signatures
  const sigY = y;

  // Left signature (Employee)
  doc.setFontSize(8);
  doc.text(`Izradio: ${data.employeeName}`, 15, sigY);
  doc.text('m.p.', 15, sigY + 10);

  // Right signature (Seller)
  doc.text('Prodavatelj:', pageWidth - 15, sigY, { align: 'right' });
  doc.text(data.seller.name, pageWidth - 15, sigY + 5, { align: 'right' });

  // Page number
  doc.setFontSize(7);
  doc.text('1 od 1', pageWidth - 15, doc.internal.pageSize.getHeight() - 10, { align: 'right' });

  // Save/download the PDF
  doc.save(`otkupni-blok-${data.documentNumber}.pdf`);
};
