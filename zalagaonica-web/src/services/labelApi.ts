import api from './api';

export interface Label {
  id: string;
  articleId: string;
  articleName: string;
  barcode: string;
  price: number;
  quantity: number;
  isPrinted: boolean;
  printedAt?: string;
  createdAt: string;
}

export const labelApi = {
  getAll: async (): Promise<Label[]> => {
    return await api.get('/Label');
  },

  getById: async (id: string): Promise<Label> => {
    return await api.get(`/Label/${id}`);
  },

  getByArticle: async (articleId: string): Promise<Label[]> => {
    return await api.get(`/Label/article/${articleId}`);
  },

  create: async (label: Partial<Label>): Promise<Label> => {
    return await api.post('/Label', label);
  },

  update: async (id: string, label: Partial<Label>): Promise<void> => {
    await api.put(`/Label/${id}`, { ...label, id });
  },

  markAsPrinted: async (id: string): Promise<void> => {
    await api.post(`/Label/${id}/print`, {});
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/Label/${id}`);
  }
};
