import api from './api';

export interface InventoryBook {
  id: string;
  articleId: string;
  articleName: string;
  date: string;
  documentNumber: string;
  inQuantity: number;
  outQuantity: number;
  balance: number;
  notes?: string;
  createdAt: string;
}

export const inventoryBookApi = {
  getAll: async (): Promise<InventoryBook[]> => {
    const response = await api.get('/InventoryBook');
    return response.data;
  },

  getById: async (id: string): Promise<InventoryBook> => {
    const response = await api.get(`/InventoryBook/${id}`);
    return response.data;
  },

  getByArticle: async (articleId: string): Promise<InventoryBook[]> => {
    const response = await api.get(`/InventoryBook/article/${articleId}`);
    return response.data;
  },

  create: async (entry: Partial<InventoryBook>): Promise<InventoryBook> => {
    const response = await api.post('/InventoryBook', entry);
    return response.data;
  },

  update: async (id: string, entry: Partial<InventoryBook>): Promise<void> => {
    await api.put(`/InventoryBook/${id}`, { ...entry, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/InventoryBook/${id}`);
  }
};
