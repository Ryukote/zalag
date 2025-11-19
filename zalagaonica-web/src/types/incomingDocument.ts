// src/types/incomingDocument.ts

export interface IncomingDocument {
  id: number;
  supplierName: string;
  bookingDate: string; // Datum knjiženja
  documentNumber: string; // Ulazni dok.
  documentDate: string; // Datum
  purchaseValue: number; // Nabavna vrijednost
  margin: number; // Marža
  tax: number; // Porez
  status: 'otvoren' | 'proknjižen';
  warehouseName: string; // Naziv skl.
  documentType: string; // Tip dokumenta
  year: number;
  operator: string;
  dueDate: string;
  isPosted: boolean;
  invoiceValue: number;
  discount: number;
  cost: number;
  wholesaleValue: number;
  vatAmount: number;
  retailValue: number;
  returnFee: number;
  totalWithReturnFee: number;
  pretaxAmount: number;
  totalPaid: number;
  note?: string;
}