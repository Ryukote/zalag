// src/types/priceListItem.ts

export interface PriceListItem {
  id: string;              // OZNAKA
  name: string;            // NAZIV
  stock: number;           // STANJE
  unitOfMeasure: string;   // MJERA
  retailPrice: number;     // MPC
  retailPriceWithTax: number; // MPCP
  group?: string;          // PROSNAB. GRUPA
  taxRate: number;         // POREZ
  taxTariffNumber?: string;  // RBR.TX
  supplierName: string;    // NAZIV DOBAVLJAČA
}