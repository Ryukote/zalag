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
    return await api.get('/InventoryBook');
  },

  getById: async (id: string): Promise<InventoryBook> => {
    return await api.get(`/InventoryBook/${id}`);
  },

  getByArticle: async (articleId: string): Promise<InventoryBook[]> => {
    return await api.get(`/InventoryBook/article/${articleId}`);
  },

  create: async (entry: Partial<InventoryBook>): Promise<InventoryBook> => {
    return await api.post('/InventoryBook', entry);
  },

  update: async (id: string, entry: Partial<InventoryBook>): Promise<void> => {
    await api.put(`/InventoryBook/${id}`, { ...entry, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/InventoryBook/${id}`);
  }
};
