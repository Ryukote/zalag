import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { robotoRegularBase64 } from './fonts/roboto-base64';

declare module 'jspdf' {
  interface jsPDF {
    autoTable: (options: any) => jsPDF;
    lastAutoTable: { finalY: number };
  }
}

export interface CompanyInfo {
  name: string;
  address: string;
  city: string;
  oib: string;
  tel: string;
}

export const defaultCompanyInfo: CompanyInfo = {
  name: 'PAWN SHOPS d.o.o.',
  address: 'P.J. Horvačanska cesta 25, Zagreb',
  city: '47000 Karlovac',
  oib: '51659874442',
  tel: '092 500 8000'
};

// Format currency to Croatian format (e.g., 250,00 €)
export const formatCurrency = (amount: number, currency: string = '€'): string => {
  return `${amount.toFixed(2).replace('.', ',')} ${currency}`;
};

// Format date to Croatian format (DD.MM.YYYY)
export const formatDate = (date: Date): string => {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
};

// Draw company header in a box (top-left corner)
export const drawCompanyHeader = (doc: jsPDF, companyInfo: CompanyInfo = defaultCompanyInfo, y: number = 15): number => {
  const lineHeight = 5;
  const boxPadding = 3;
  const boxX = 15;
  const boxWidth = 85;

  // Title
  doc.setFontSize(9);
  doc.setFont('courier', 'bold');
  doc.text('KUPAC:', boxX, y);

  y += lineHeight;

  // Company info
  doc.setFont('courier', 'normal');
  doc.setFontSize(8);

  const companyLines = [
    companyInfo.name,
    companyInfo.address,
    `Logorište 11a`,
    companyInfo.city,
    `OIB: ${companyInfo.oib}`,
    `Tel: ${companyInfo.tel}`
  ];

  const textStartY = y + boxPadding;
  const boxHeight = (companyLines.length * lineHeight) + (boxPadding * 2);

  // Draw box
  doc.rect(boxX, y, boxWidth, boxHeight);

  // Draw text
  companyLines.forEach((line, index) => {
    doc.text(line, boxX + boxPadding, textStartY + (index * lineHeight));
  });

  return y + boxHeight + 5;
};

// Draw client/seller info (top-right corner)
export const drawClientInfo = (
  doc: jsPDF,
  clientInfo: { name: string; address?: string; city?: string; oib?: string; },
  y: number = 15,
  label: string = 'Prodavatelj:'
): number => {
  const lineHeight = 5;
  const pageWidth = doc.internal.pageSize.getWidth();
  const boxX = pageWidth - 100;

  doc.setFontSize(9);
  doc.setFont('courier', 'bold');
  doc.text(label, boxX, y);

  y += lineHeight;

  doc.setFont('courier', 'normal');
  doc.setFontSize(8);

  let currentY = y;
  if (clientInfo.name) {
    doc.text(clientInfo.name, boxX, currentY);
    currentY += lineHeight;
  }
  if (clientInfo.address) {
    doc.text(clientInfo.address, boxX, currentY);
    currentY += lineHeight;
  }
  if (clientInfo.city) {
    doc.text(clientInfo.city, boxX, currentY);
    currentY += lineHeight;
  }
  if (clientInfo.oib) {
    doc.text(`OIB: ${clientInfo.oib}`, boxX, currentY);
    currentY += lineHeight;
  }

  return currentY + 5;
};

// Draw document metadata (dates, document number, etc.)
export const drawDocumentInfo = (
  doc: jsPDF,
  info: { [key: string]: string },
  y: number
): number => {
  const lineHeight = 5;
  const pageWidth = doc.internal.pageSize.getWidth();
  const rightX = pageWidth - 15;

  doc.setFontSize(8);
  doc.setFont('courier', 'normal');

  Object.entries(info).forEach(([label, value]) => {
    const text = `${label}: ${value}`;
    doc.text(text, rightX, y, { align: 'right' });
    y += lineHeight;
  });

  return y + 3;
};

// Draw centered title
export const drawTitle = (doc: jsPDF, title: string, y: number): number => {
  const pageWidth = doc.internal.pageSize.getWidth();

  doc.setFontSize(14);
  doc.setFont('courier', 'bold');
  doc.text(title, pageWidth / 2, y, { align: 'center' });

  return y + 10;
};

// Draw a multi-line text block
export const drawTextBlock = (
  doc: jsPDF,
  text: string,
  x: number,
  y: number,
  maxWidth: number = 180
): number => {
  doc.setFontSize(8);
  doc.setFont('courier', 'normal');

  const lines = doc.splitTextToSize(text, maxWidth);
  doc.text(lines, x, y);

  return y + (lines.length * 5) + 5;
};

// Draw signature section
export const drawSignatureSection = (
  doc: jsPDF,
  y: number,
  leftLabel: string = 'Izradio:',
  rightLabel: string = 'Prodavatelj:'
): number => {
  const lineHeight = 5;
  const pageWidth = doc.internal.pageSize.getWidth();

  doc.setFontSize(8);
  doc.setFont('courier', 'normal');

  // Left signature (Izradio)
  doc.text(`${leftLabel} ___________________`, 15, y);
  y += lineHeight;
  doc.text('m.p.', 15, y);

  // Right signature (Prodavatelj/Client)
  doc.text(`${rightLabel}`, pageWidth - 15, y - lineHeight, { align: 'right' });
  doc.text('___________________', pageWidth - 15, y, { align: 'right' });

  return y + 10;
};

// Initialize a new PDF document with UTF-8 support for Croatian characters
export const createPDF = (orientation: 'portrait' | 'landscape' = 'portrait'): jsPDF => {
  const doc = new jsPDF({
    orientation,
    unit: 'mm',
    format: 'a4',
    putOnlyUsedFonts: true,
    compress: true
  });

  // Add Roboto font for Croatian character support
  if (robotoRegularBase64 && typeof robotoRegularBase64 === 'string' && robotoRegularBase64.length > 100) {
    try {
      doc.addFileToVFS('Roboto-Regular.ttf', robotoRegularBase64);
      doc.addFont('Roboto-Regular.ttf', 'Roboto', 'normal');
      doc.setFont('Roboto', 'normal');
    } catch (error) {
      console.warn('Failed to load Roboto font, using courier fallback:', error);
      doc.setFont('courier', 'normal');
    }
  } else {
    // Fallback to courier if Roboto font is not available
    doc.setFont('courier', 'normal');
  }

  return doc;
};

// Calculate VAT amount
export const calculateVAT = (amount: number, vatRate: number): number => {
  return amount * (vatRate / 100);
};

// Calculate gross amount (amount + VAT)
export const calculateGross = (net: number, vatRate: number): number => {
  return net + calculateVAT(net, vatRate);
};

// Calculate net amount from gross
export const calculateNet = (gross: number, vatRate: number): number => {
  return gross / (1 + (vatRate / 100));
};
