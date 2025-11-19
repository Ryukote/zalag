export interface CashRegisterTransaction {
  id: string;
  date: string;
  amount: number;
  type: string;
  description?: string;
}