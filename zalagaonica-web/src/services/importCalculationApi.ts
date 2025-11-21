import api from './api';

export interface ImportCalculation {
  id: string;
  documentNumber: string;
  date: string;
  supplierName: string;
  totalGoods: number;
  customsDuty: number;
  transportCost: number;
  otherCosts: number;
  totalCost: number;
  createdAt: string;
}

export const importCalculationApi = {
  getAll: async (): Promise<ImportCalculation[]> => {
    const response = await api.get('/ImportCalculation');
    return response.data;
  },

  getById: async (id: string): Promise<ImportCalculation> => {
    const response = await api.get(`/ImportCalculation/${id}`);
    return response.data;
  },

  create: async (calculation: Partial<ImportCalculation>): Promise<ImportCalculation> => {
    const response = await api.post('/ImportCalculation', calculation);
    return response.data;
  },

  update: async (id: string, calculation: Partial<ImportCalculation>): Promise<void> => {
    await api.put(`/ImportCalculation/${id}`, { ...calculation, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/ImportCalculation/${id}`);
  }
};
