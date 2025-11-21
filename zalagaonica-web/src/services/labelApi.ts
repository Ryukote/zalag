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
    const response = await api.get('/Label');
    return response.data;
  },

  getById: async (id: string): Promise<Label> => {
    const response = await api.get(`/Label/${id}`);
    return response.data;
  },

  getByArticle: async (articleId: string): Promise<Label[]> => {
    const response = await api.get(`/Label/article/${articleId}`);
    return response.data;
  },

  create: async (label: Partial<Label>): Promise<Label> => {
    const response = await api.post('/Label', label);
    return response.data;
  },

  update: async (id: string, label: Partial<Label>): Promise<void> => {
    await api.put(`/Label/${id}`, { ...label, id });
  },

  markAsPrinted: async (id: string): Promise<void> => {
    await api.post(`/Label/${id}/print`);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/Label/${id}`);
  }
};
