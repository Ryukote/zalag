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
    return await api.get('/PriceChangeLog');
  },

  getById: async (id: string): Promise<PriceChangeLog> => {
    return await api.get(`/PriceChangeLog/${id}`);
  },

  getByArticle: async (articleId: string): Promise<PriceChangeLog[]> => {
    return await api.get(`/PriceChangeLog/article/${articleId}`);
  },

  create: async (log: Partial<PriceChangeLog>): Promise<PriceChangeLog> => {
    return await api.post('/PriceChangeLog', log);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/PriceChangeLog/${id}`);
  }
};
