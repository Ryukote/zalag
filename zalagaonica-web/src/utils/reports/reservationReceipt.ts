import {
  createPDF,
  drawCompanyHeader,
  drawTitle,
  drawTextBlock,
  formatCurrency,
  formatDate,
  defaultCompanyInfo
} from '../pdfGenerator';

export interface ReservationReceiptData {
  documentNumber: string;
  documentDate: Date;
  buyer: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  warehouse: string;
  items: Array<{
    name: string;
    code: string;
    quantity: number;
    unitOfMeasure: string;
    pricePerUnit: number;
    mpc: number;
  }>;
  totalAmount: number;
  exchangeRate: number;
  totalInKuna: number;
  reservationDeposit: number;
}

export const generateReservationReceipt = (data: ReservationReceiptData): void => {
  const doc = createPDF();
  let y = 15;

  // Company header (left)
  y = drawCompanyHeader(doc, defaultCompanyInfo, y);
  y += 5;

  // Buyer info (right)
  const pageWidth = doc.internal.pageSize.getWidth();
  let rightY = 15;
  doc.setFontSize(9);
  doc.setFont('helvetica', 'bold');
  doc.text('Prodavatelj:', pageWidth - 100, rightY);
  rightY += 5;

  doc.setFont('helvetica', 'normal');
  doc.setFontSize(8);
  doc.text(data.buyer.name, pageWidth - 100, rightY);
  rightY += 5;
  if (data.buyer.address) {
    doc.text(data.buyer.address, pageWidth - 100, rightY);
    rightY += 5;
  }
  if (data.buyer.city) {
    doc.text(data.buyer.city, pageWidth - 100, rightY);
    rightY += 5;
  }
  if (data.buyer.oib) {
    doc.text(`OIB: ${data.buyer.oib}`, pageWidth - 100, rightY);
    rightY += 5;
  }

  // Document date
  doc.setFontSize(8);
  doc.text(`Datum dokumenta: ${formatDate(data.documentDate)}`, 15, y);
  y += 5;
  doc.text(`Skladište: ${data.warehouse}`, 15, y);

  y = Math.max(y, rightY) + 10;

  // Title
  y = drawTitle(doc, `OTKUPNI BLOK RAB. DOBRA S REZERVACIJOM: ${data.documentNumber}`, y);
  y += 5;

  // Items table
  const tableData = data.items.map((item, index) => [
    index + 1,
    item.name,
    item.code,
    item.unitOfMeasure,
    item.quantity,
    formatCurrency(item.pricePerUnit),
    formatCurrency(item.mpc)
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
      1: { cellWidth: 50 },
      2: { cellWidth: 30 },
      3: { halign: 'center', cellWidth: 20 },
      4: { halign: 'center', cellWidth: 20 },
      5: { halign: 'right', cellWidth: 25 },
      6: { halign: 'right', cellWidth: 25 }
    }
  });

  y = doc.lastAutoTable.finalY + 5;

  // Total amount
  doc.setFontSize(10);
  doc.setFont('helvetica', 'bold');
  doc.text(`Ukupno: ${formatCurrency(data.totalAmount)}`, pageWidth - 15, y, { align: 'right' });
  y += 5;

  // Exchange rate info
  doc.setFontSize(8);
  doc.setFont('helvetica', 'normal');
  const exchangeText = `prema tečaju: 1 euro = ${data.exchangeRate.toFixed(5)} kuna`;
  doc.text(exchangeText, pageWidth - 15, y, { align: 'right' });
  y += 5;
  doc.text(`Ukupno: ${formatCurrency(data.totalInKuna, 'kn')}`, pageWidth - 15, y, { align: 'right' });

  y += 10;

  // Legal statement
  const legalStatement = `Izjava prodavatelja:
Izjavljujem da je predmet isključivo moje vlasništvo.
Suglasan sam da: PAWN SHOPS d.o.o. ne odgovora za dokumente, podatke, informacije, autorska i vlasnička prava trećih osoba te bilo koji drugi sadržaj, kao ni mogućih štetu prema prodavatelju ili bilo kojoj trećoj strani koja bi mogla nastati zbog upotrebe istih.

Izjavljujem da nisam porezni obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, niti sam obveznik fiskalizacije.

REZERVACIJA ROBE: PRAVO OTKUPA ROBE DO DATUMA ISTEKA REZERVACIJE O
Kapara za pravo rezervacije i pravo otkupa nije plaćena.

PRODAVATELJ i KUPAC sporazumno utvrđuju da će PRODAVATELJ naslaviti OTKUP po ovoj rezervaciji 24 sata ranije.
Kupac se obavezuje da PLU-otplati OTKUP od OTPLATE bez dužih prava proizvoda s čijelosti do isteka rezervacije, KUPAC zadržava robu, te obje strane nemaju međusobnih potražlvanja vezano uz ovu kupovinu.
U slučaju ispunjenja svih uvjeta otplate i poštivanje rezervacije marže po članku 9S.a Zakona o porezu na dodanu vrijednost.
Roba je kapirana po principu viđeno-kupljeno te su KUPAC i PRODAVATELJ sami dužni utvrdili ispravnost proizvoda prije kupnje.

PRODAVATELJ i KUPAC sporazumno utvrđuju da će PRODAVATELJ naslaviti OTKUP po ovoj rezervaciji 24 sata ranije.`;

  y = drawTextBlock(doc, legalStatement, 15, y, 180);

  // Signature section
  doc.setFontSize(8);
  doc.setFont('helvetica', 'normal');
  doc.text('Izradio: Tin Matija', 15, y);
  doc.text('m.p.', 15, y + 5);

  doc.text('Prodavatelj:', pageWidth - 15, y, { align: 'right' });
  doc.text('Prodavatelj:', pageWidth - 15, y + 5, { align: 'right' });
  doc.text(data.buyer.name, pageWidth - 15, y + 10, { align: 'right' });

  y += 20;

  // Reservation deposit
  doc.setFontSize(9);
  doc.setFont('helvetica', 'bold');
  doc.text(`Kapara pri otkupu: ${formatCurrency(data.reservationDeposit)}`, 15, y);

  // Add page number
  doc.setFontSize(7);
  doc.setFont('helvetica', 'italic');
  doc.text('1 od 1', pageWidth - 15, doc.internal.pageSize.getHeight() - 10, { align: 'right' });

  // Save/download the PDF
  doc.save(`otkupni-blok-rezervacija-${data.documentNumber}.pdf`);
};
