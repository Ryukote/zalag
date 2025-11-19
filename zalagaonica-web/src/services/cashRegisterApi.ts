import { api } from './api';
import { CashRegisterTransaction } from '../types/cashRegisterTransaction';

export type { CashRegisterTransaction };

export const cashRegisterApi = {
  getAll: () => api.get<CashRegisterTransaction[]>('/CashRegisterTransaction'),

  getById: (id: string) => api.get<CashRegisterTransaction>(`/CashRegisterTransaction/${id}`),

  create: (transaction: Omit<CashRegisterTransaction, 'id'>) =>
    api.post<CashRegisterTransaction>('/CashRegisterTransaction', transaction),

  update: (id: string, transaction: CashRegisterTransaction) =>
    api.put<void>(`/CashRegisterTransaction/${id}`, transaction),

  delete: (id: string) =>
    api.delete(`/CashRegisterTransaction/${id}`),
};
