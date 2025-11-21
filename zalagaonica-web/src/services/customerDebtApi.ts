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
    const response = await api.get('/CustomerDebt');
    return response.data;
  },

  getOverdue: async (): Promise<CustomerDebt[]> => {
    const response = await api.get('/CustomerDebt/overdue');
    return response.data;
  },

  getById: async (id: string): Promise<CustomerDebt> => {
    const response = await api.get(`/CustomerDebt/${id}`);
    return response.data;
  },

  create: async (debt: Partial<CustomerDebt>): Promise<CustomerDebt> => {
    const response = await api.post('/CustomerDebt', debt);
    return response.data;
  },

  update: async (id: string, debt: Partial<CustomerDebt>): Promise<void> => {
    await api.put(`/CustomerDebt/${id}`, { ...debt, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/CustomerDebt/${id}`);
  }
};
