import { api } from './api';

export interface Loan {
  id: string;
  clientId: string;
  articleId: string;
  amount: number;
  interestRate: number;
  startDate: string;
  endDate?: string;
  status: string;
}

export const loanApi = {
  getAll: () => api.get<Loan[]>('/Loan'),

  getById: (id: string) => api.get<Loan>(`/Loan/${id}`),

  create: (loan: Omit<Loan, 'id'>) =>
    api.post<Loan>('/Loan', loan),

  update: (id: string, loan: Loan) =>
    api.put<void>(`/Loan/${id}`, loan),

  delete: (id: string) =>
    api.delete(`/Loan/${id}`),
};
