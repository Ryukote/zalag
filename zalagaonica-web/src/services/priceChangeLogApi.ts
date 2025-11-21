import api from './api';

export interface PriceChangeLog {
  id: string;
  articleId: string;
  articleName: string;
  oldPrice: number;
  newPrice: number;
  changeDate: string;
  reason: string;
  changedBy: string;
  createdAt: string;
}

export const priceChangeLogApi = {
  getAll: async (): Promise<PriceChangeLog[]> => {
    const response = await api.get('/PriceChangeLog');
    return response.data;
  },

  getById: async (id: string): Promise<PriceChangeLog> => {
    const response = await api.get(`/PriceChangeLog/${id}`);
    return response.data;
  },

  getByArticle: async (articleId: string): Promise<PriceChangeLog[]> => {
    const response = await api.get(`/PriceChangeLog/article/${articleId}`);
    return response.data;
  },

  create: async (log: Partial<PriceChangeLog>): Promise<PriceChangeLog> => {
    const response = await api.post('/PriceChangeLog', log);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/PriceChangeLog/${id}`);
  }
};
