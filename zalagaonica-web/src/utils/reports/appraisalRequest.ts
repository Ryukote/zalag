import {
  createPDF,
  drawCompanyHeader,
  drawClientInfo,
  drawTitle,
  drawTextBlock,
  formatDate,
  defaultCompanyInfo
} from '../pdfGenerator';

export interface AppraisalRequestData {
  documentNumber: string;
  documentDate: Date;
  client: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  items: Array<{
    name: string;
    description: string;
    code: string;
    quantity: number;
    unitOfMeasure: string;
  }>;
  warehouse: string;
}

export const generateAppraisalRequest = (data: AppraisalRequestData): void => {
  const doc = createPDF();
  let y = 15;

  // Company header (left)
  y = drawCompanyHeader(doc, defaultCompanyInfo, y);
  y += 5;

  // Client info (right)
  drawClientInfo(doc, data.client, 15, 'Prodavatelj:');

  // Document metadata
  const pageWidth = doc.internal.pageSize.getWidth();
  let rightY = 30;
  doc.setFontSize(8);
  doc.text(`Datum ul. dokumenta: ${formatDate(data.documentDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum isteka:`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Skladište: ${data.warehouse}`, pageWidth - 15, rightY, { align: 'right' });

  y = Math.max(y, rightY) + 10;

  // Title
  y = drawTitle(doc, `ZAHTJEV ZA PROCJENOM PREDMETA: ${data.documentNumber}`, y);
  y += 5;

  // Introduction text
  const introText = `Molim tvrtku PAWN SHOPS d.o.o. da u svrhu mogućeg otkupa predmeta u mom vlasništvu, izvrši potrebna ispitivanja (vizualna, termička, kemijska ili ...), kako bi utvrdila kakvoću predmeta koje nudim.
Prihvaćam činjenicom da tvrtka PAWN SHOPS d.o.o. nije dužna plaćati ni kakvoću predmeta po izvršenim provjerama ili procjenama ili procjeni izvršiti otkup predmeta koje nudim i na kojima je radila procjena.`;

  y = drawTextBlock(doc, introText, 15, y, 180);
  y += 10;

  // Items table
  const tableData = data.items.map((item, index) => [
    index + 1,
    item.name,
    item.code,
    item.unitOfMeasure,
    item.quantity
  ]);

  doc.autoTable({
    startY: y,
    head: [['', 'naziv artikla', 'oznaka / šifra', 'jed. mjere', 'količina']],
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
      1: { cellWidth: 80 },
      2: { cellWidth: 40 },
      3: { halign: 'center', cellWidth: 25 },
      4: { halign: 'center', cellWidth: 25 }
    }
  });

  y = doc.lastAutoTable.finalY + 10;

  // Employee signature
  doc.setFontSize(8);
  doc.text('IZRADIO: Domagoj', 15, y);
  y += 5;
  doc.text('m.p.', 15, y);
  y += 5;

  // Client label on right
  doc.text('KOMITENT', pageWidth - 15, y - 10, { align: 'right' });

  y += 5;

  // Legal statements
  doc.setFontSize(8);
  doc.text('Izjava vlasništva:', 15, y);
  y += 5;

  const ownershipStatement = `Izjavljujem da je predmet koji prodajem isključivo moje vlasništvo. Izjavljujem da nisam porezni obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, niti sam obveznik fiskalizacije.`;
  y = drawTextBlock(doc, ownershipStatement, 15, y, 180);
  y += 5;

  doc.text('Uvjeti procjene:', 15, y);
  y += 5;

  const appraisalTerms = `Kupac je dužan robu za procjene podići u roku od 30 dana. Ukoliko se roba sa procjene ne podigne u roku od 30 dana, tvrtka PAWN SHOPS d.o.o. zadržava robu, te obje strane nemaju međusobnih potraživanja, vezano uz ovu transakciju.`;
  y = drawTextBlock(doc, appraisalTerms, 15, y, 180);

  // Add page number
  doc.setFontSize(7);
  doc.text('1 od 1', pageWidth - 15, doc.internal.pageSize.getHeight() - 10, { align: 'right' });

  // Save/download the PDF
  doc.save(`zahtjev-procjena-${data.documentNumber}.pdf`);
};
