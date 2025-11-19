import { api } from './api';
import { Payroll } from '../types/HR';

export type { Payroll };

export const payrollApi = {
  getAll: () => api.get<Payroll[]>('/Payroll'),

  getById: (id: string) => api.get<Payroll>(`/Payroll/${id}`),

  create: (payroll: Omit<Payroll, 'id'>) =>
    api.post<Payroll>('/Payroll', payroll),

  update: (id: string, payroll: Payroll) =>
    api.put<void>(`/Payroll/${id}`, payroll),

  delete: (id: string) =>
    api.delete(`/Payroll/${id}`),
};
