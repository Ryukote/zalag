import api from './api';

export interface InventoryCount {
  id: string;
  date: string;
  description: string;
  isApproved: boolean;
  approvedBy?: string;
  approvedAt?: string;
  totalItems: number;
  totalValue: number;
  createdAt: string;
}

export const inventoryCountApi = {
  getAll: async (): Promise<InventoryCount[]> => {
    return await api.get('/InventoryCount');
  },

  getById: async (id: string): Promise<InventoryCount> => {
    return await api.get(`/InventoryCount/${id}`);
  },

  create: async (count: Partial<InventoryCount>): Promise<InventoryCount> => {
    return await api.post('/InventoryCount', count);
  },

  update: async (id: string, count: Partial<InventoryCount>): Promise<void> => {
    await api.put(`/InventoryCount/${id}`, { ...count, id });
  },

  approve: async (id: string): Promise<void> => {
    await api.post(`/InventoryCount/${id}/approve`, {});
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/InventoryCount/${id}`);
  }
};
