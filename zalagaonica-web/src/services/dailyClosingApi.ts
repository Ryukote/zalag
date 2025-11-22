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
    return await api.get('/DailyClosing');
  },

  getById: async (id: string): Promise<DailyClosing> => {
    return await api.get(`/DailyClosing/${id}`);
  },

  getByDate: async (date: string): Promise<DailyClosing> => {
    return await api.get(`/DailyClosing/date/${date}`);
  },

  isDateClosed: async (date: string): Promise<{ date: string; isClosed: boolean }> => {
    return await api.get(`/DailyClosing/check/${date}`);
  },

  create: async (closing: Partial<DailyClosing>): Promise<DailyClosing> => {
    return await api.post('/DailyClosing', closing);
  },

  update: async (id: string, closing: Partial<DailyClosing>): Promise<void> => {
    await api.put(`/DailyClosing/${id}`, { ...closing, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/DailyClosing/${id}`);
  }
};
