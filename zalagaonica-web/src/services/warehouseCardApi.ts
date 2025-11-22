import api from './api';

export interface WarehouseCard {
  id: string;
  articleId: string;
  articleName: string;
  date: string;
  documentType: string;
  documentNumber: string;
  inQuantity: number;
  outQuantity: number;
  balance: number;
  notes?: string;
  createdAt: string;
}

export const warehouseCardApi = {
  getAll: async (): Promise<WarehouseCard[]> => {
    return await api.get('/WarehouseCard');
  },

  getById: async (id: string): Promise<WarehouseCard> => {
    return await api.get(`/WarehouseCard/${id}`);
  },

  getByArticle: async (articleId: string): Promise<WarehouseCard[]> => {
    return await api.get(`/WarehouseCard/article/${articleId}`);
  },

  create: async (card: Partial<WarehouseCard>): Promise<WarehouseCard> => {
    return await api.post('/WarehouseCard', card);
  },

  update: async (id: string, card: Partial<WarehouseCard>): Promise<void> => {
    await api.put(`/WarehouseCard/${id}`, { ...card, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/WarehouseCard/${id}`);
  }
};
