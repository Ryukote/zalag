// src/types/client.ts

export interface Client {
  id: number;
  name: string;
  city: string;
  address: string;
  taxId: string; // Ovo je bio 'OIB'
  phone?: string;
  fax?: string;
  mobile?: string;
  email: string;
  bankAccountNumber?: string; // Ovo je bio 'Žiro-račun'
  iban: string;
  swift?: string;
  type: 'individual' | 'legal'; // 'F' | 'P' -> Fizička/Pravna
  status: 'active' | 'inactive';
  createdBy?: string;
  createdAt: string;
  updatedBy?: string;
  updatedAt: string;
}