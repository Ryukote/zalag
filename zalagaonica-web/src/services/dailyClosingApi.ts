import api from './api';

export interface DailyClosing {
  id: string;
  date: string;
  cashierName: string;
  startingCash: number;
  totalSales: number;
  totalExpenses: number;
  cashInRegister: number;
  isClosed: boolean;
  closedAt?: string;
  notes?: string;
  createdAt: string;
}

export const dailyClosingApi = {
  getAll: async (): Promise<DailyClosing[]> => {
    const response = await api.get('/DailyClosing');
    return response.data;
  },

  getById: async (id: string): Promise<DailyClosing> => {
    const response = await api.get(`/DailyClosing/${id}`);
    return response.data;
  },

  getByDate: async (date: string): Promise<DailyClosing> => {
    const response = await api.get(`/DailyClosing/date/${date}`);
    return response.data;
  },

  isDateClosed: async (date: string): Promise<{ date: string; isClosed: boolean }> => {
    const response = await api.get(`/DailyClosing/check/${date}`);
    return response.data;
  },

  create: async (closing: Partial<DailyClosing>): Promise<DailyClosing> => {
    const response = await api.post('/DailyClosing', closing);
    return response.data;
  },

  update: async (id: string, closing: Partial<DailyClosing>): Promise<void> => {
    await api.put(`/DailyClosing/${id}`, { ...closing, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/DailyClosing/${id}`);
  }
};
