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
    const response = await api.get('/WarehouseCard');
    return response.data;
  },

  getById: async (id: string): Promise<WarehouseCard> => {
    const response = await api.get(`/WarehouseCard/${id}`);
    return response.data;
  },

  getByArticle: async (articleId: string): Promise<WarehouseCard[]> => {
    const response = await api.get(`/WarehouseCard/article/${articleId}`);
    return response.data;
  },

  create: async (card: Partial<WarehouseCard>): Promise<WarehouseCard> => {
    const response = await api.post('/WarehouseCard', card);
    return response.data;
  },

  update: async (id: string, card: Partial<WarehouseCard>): Promise<void> => {
    await api.put(`/WarehouseCard/${id}`, { ...card, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/WarehouseCard/${id}`);
  }
};
