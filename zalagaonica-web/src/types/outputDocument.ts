// src/types/outputDocument.ts

/**
 * Sučelje koje predstavlja izlazni dokument (npr. račun, otpremnica).
 */
export interface OutputDocument {
  id: string;                 // Jedinstveni identifikator dokumenta
  documentNumber: string;     // Broj dokumenta (npr. broj računa)
  documentDate: string;       // Datum izdavanja dokumenta
  clientName: string;         // Naziv klijenta/partnera
  documentType: string;       // Tip dokumenta (npr. 'Račun', 'Otpremnica')
  warehouseName: string;      // Naziv skladišta iz kojeg se roba otprema
  totalValue: number;         // Ukupna vrijednost dokumenta bez poreza
  taxAmount: number;          // Ukupan iznos poreza
  totalWithTax: number;       // Ukupna vrijednost s porezom
  status: 'draft' | 'posted' | 'canceled'; // Status dokumenta
  operator: string;           // Korisnik koji je kreirao ili zadnji put mijenjao dokument
  year: number;               // Poslovna godina dokumenta
  note?: string;              // Dodatne bilješke ili komentari
  isPosted: boolean;          // Oznaka je li dokument proknjižen
  dueDate?: string;           // Rok za plaćanje, ako je primjenjivo
  discount?: number;          // Ukupan iznos popusta
  type: 'individual' | 'legal';
  customerName?: string;
  
  // Nova polja iz GUI-a
  paymentType: 'cash' | 'card' | 'bank_transfer' | 'other'; // Tip plaćanja
  isPaid: boolean;            // Je li račun plaćen
  paymentDate?: string;       // Datum plaćanja
  shippingAddress?: string;   // Adresa za dostavu
  currency: string;           // Valuta dokumenta (npr. 'EUR', 'USD')
  
  items: OutputDocumentItem[]; // Lista stavki
  
  discountPercentage?: number; // Rabat u postocima
  totalDiscountAmount?: number; // Ukupan iznos rabata
  totalRetailPrice: number; // Ukupna maloprodajna cijena (MPCP)
  totalTaxAmount: number; // Ukupan iznos poreza
  totalWithoutTax: number; // Ukupna vrijednost bez poreza
}

/**
 * Sučelje koje predstavlja pojedinačnu stavku na izlaznom dokumentu.
 */
export interface OutputDocumentItem {
  id: string;                 // Jedinstveni ID stavke
  articleId: string;          // Oznaka artikla
  name: string;               // Naziv artikla
  unitOfMeasure: string;      // Mjerna jedinica
  quantity: number;           // Količina
  retailPrice: number;        // Maloprodajna cijena (MPC)
  taxRate: number;            // Stopa poreza u postocima
  totalPrice: number;         // Ukupna vrijednost stavke bez poreza (količina * cijena)
  totalWithTax: number;       // Ukupna vrijednost stavke s porezom
  taxAmount: number;          // Iznos poreza za stavku
  discountPercentage: number; // Rabat u postocima
  discountAmount: number;     // Iznos rabata
}