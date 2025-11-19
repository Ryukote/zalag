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

export interface PledgeAgreementData {
  pledgeNumber: string;
  pledgeDate: Date;
  client: {
    name: string;
    address?: string;
    city?: string;
    oib?: string;
  };
  item: {
    name: string;
    description: string;
    estimatedValue: number;
  };
  loanAmount: number;
  returnAmount: number;
  period: number;
  redeemDeadline: Date;
  interestRate?: number;
}

export const generatePledgeAgreement = (data: PledgeAgreementData): void => {
  const doc = createPDF();
  let y = 15;

  // Company header (left)
  y = drawCompanyHeader(doc, defaultCompanyInfo, y);
  y += 5;

  // Client info (right)
  drawClientInfo(doc, data.client, 15, 'Zaloglitelj:');

  // Document metadata
  const pageWidth = doc.internal.pageSize.getWidth();
  let rightY = 30;
  doc.setFontSize(8);
  doc.text(`Broj ugovora: ${data.pledgeNumber}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Datum: ${formatDate(data.pledgeDate)}`, pageWidth - 15, rightY, { align: 'right' });
  rightY += 5;
  doc.text(`Rok isplate: ${formatDate(data.redeemDeadline)}`, pageWidth - 15, rightY, { align: 'right' });

  y = Math.max(y, rightY) + 10;

  // Title
  y = drawTitle(doc, `UGOVOR O ZALOGU - ${data.pledgeNumber}`, y);
  y += 5;

  // Introduction
  doc.setFontSize(8);
  const introText = `Ovaj ugovor o zalogu zaključen je između tvrtke PAWN SHOPS d.o.o. (dalje u tekstu: Zalagaonica) i gore navedenog zaloglitelja u skladu sa Zakonom o zalagaonicama i Općim uvjetima poslovanja.`;
  y = drawTextBlock(doc, introText, 15, y, 180);
  y += 10;

  // Section 1: Pledged Item
  doc.setFontSize(9);
  doc.text('ČLANAK 1. - PREDMET ZALOGA', 15, y);
  y += 6;

  doc.setFontSize(8);

  // Item details table
  const itemDetails = [
    ['Naziv predmeta:', data.item.name],
    ['Opis i stanje:', data.item.description],
    ['Procijenjena vrijednost:', formatCurrency(data.item.estimatedValue)]
  ];

  itemDetails.forEach(([label, value]) => {
      doc.text(label, 20, y);
      const lines = doc.splitTextToSize(value, 140);
    doc.text(lines, 70, y);
    y += Math.max(5, lines.length * 5);
  });

  y += 5;

  // Section 2: Loan Terms
  doc.setFontSize(9);
  doc.text('ČLANAK 2. - UVJETI ZAJMA', 15, y);
  y += 6;

  doc.setFontSize(8);

  const loanDetails = [
    ['Iznos pozajmice:', formatCurrency(data.loanAmount)],
    ['Iznos za isplatu:', formatCurrency(data.returnAmount)],
    ['Razdoblje:', `${data.period} dana`],
    ['Rok isplate:', formatDate(data.redeemDeadline)]
  ];

  loanDetails.forEach(([label, value]) => {
      doc.text(label, 20, y);
      doc.text(value, 70, y);
    y += 5;
  });

  y += 5;

  // Section 3: Terms and Conditions
  doc.setFontSize(9);
  doc.text('ČLANAK 3. - OPĆI UVJETI I PRAVA STRANAKA', 15, y);
  y += 6;

  doc.setFontSize(8);

  const terms = [
    '1. Zaloglitelj izjavljuje da je isključivi vlasnik predmeta zaloga i da predmet nije opterećen pravima trećih osoba.',
    '2. Zaloglitelj se obvezuje da će vratiti pozajmljeni iznos zajedno s kamatom u dogovorenom roku.',
    '3. Zalagaonica se obvezuje da će čuvati predmet zaloga s dužnom pažnjom i osigurati ga od uništenja i oštećenja.',
    '4. Zaloglitelj ima pravo otkupiti predmet zaloga u bilo kojem trenutku tijekom trajanja ugovora plaćanjem iznosa za isplatu.',
    '5. U slučaju neisplate u dogovorenom roku, Zalagaonica stječe pravo prodaje predmeta zaloga radi naplate potraživanja.',
    '6. Ako vrijednost prodaje predmeta prelazi iznos duga, razlika pripada zaloglitelju i može se isplatiti na zahtjev.',
    '7. Ako vrijednost prodaje ne pokriva dug, Zalagaonica zadržava pravo naplate ostatka duga od zaloglitelja.',
    '8. Zaloglitelj može produljiti rok isplate ugovorom o obnovi uz plaćanje dodatnih kamata.'
  ];

  terms.forEach(term => {
    const lines = doc.splitTextToSize(term, 175);
    doc.text(lines, 15, y);
    y += lines.length * 5 + 2;
  });

  y += 5;

  // Section 4: Special Provisions
  doc.setFontSize(9);
  doc.text('ČLANAK 4. - POSEBNE ODREDBE', 15, y);
  y += 6;

  doc.setFontSize(8);
  const specialProvisions = `Zaloglitelj je upoznat s Općim uvjetima poslovanja Zalagaonice koji čine sastavni dio ovog ugovora. Zaloglitelj potvrđuje da je primio kopiju ovog ugovora te da su mu objašnjena sva prava i obveze.`;
  y = drawTextBlock(doc, specialProvisions, 15, y, 180);
  y += 10;

  // Signatures
  doc.setFontSize(8);

  const sigY = y + 10;

  // Left signature (Company)
  doc.text('ZALAGAONICA:', 15, sigY);
  doc.text('Izradio: ___________________', 15, sigY + 5);
  doc.text('m.p.', 15, sigY + 10);

  // Right signature (Client)
  doc.text('ZALOGLITELJ:', pageWidth - 15, sigY, { align: 'right' });
  doc.text('___________________', pageWidth - 15, sigY + 5, { align: 'right' });
  doc.text('(potpis)', pageWidth - 15, sigY + 10, { align: 'right' });

  // Footer with page number
  doc.setFontSize(7);
  doc.text('1 od 1', pageWidth - 15, doc.internal.pageSize.getHeight() - 10, { align: 'right' });
  doc.text(`Datum ispisa: ${formatDate(new Date())}`, 15, doc.internal.pageSize.getHeight() - 10);

  // Save/download the PDF
  doc.save(`ugovor-zalog-${data.pledgeNumber}.pdf`);
};
