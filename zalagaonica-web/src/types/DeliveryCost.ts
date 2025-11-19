export interface DeliveryCost {
  id: string;
  date: string;
  courier: 'GLS' | 'DPD' | 'Hrvatska Pošta' | 'Ostalo';
  trackingNumber: string;
  description: string;
  cost: number;
}
