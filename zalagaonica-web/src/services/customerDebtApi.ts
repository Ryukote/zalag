import api from './api';

export interface CustomerDebt {
  id: string;
  customerName: string;
  customerCode: string;
  totalDebt: number;
  paid: number;
  remaining: number;
  dueDate: string;
  status: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}

export const customerDebtApi = {
  getAll: async (): Promise<CustomerDebt[]> => {
    return await api.get('/CustomerDebt');
  },

  getOverdue: async (): Promise<CustomerDebt[]> => {
    return await api.get('/CustomerDebt/overdue');
  },

  getById: async (id: string): Promise<CustomerDebt> => {
    return await api.get(`/CustomerDebt/${id}`);
  },

  create: async (debt: Partial<CustomerDebt>): Promise<CustomerDebt> => {
    return await api.post('/CustomerDebt', debt);
  },

  update: async (id: string, debt: Partial<CustomerDebt>): Promise<void> => {
    await api.put(`/CustomerDebt/${id}`, { ...debt, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/CustomerDebt/${id}`);
  }
};
