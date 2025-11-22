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
    return await api.get('/ImportCalculation');
  },

  getById: async (id: string): Promise<ImportCalculation> => {
    return await api.get(`/ImportCalculation/${id}`);
  },

  create: async (calculation: Partial<ImportCalculation>): Promise<ImportCalculation> => {
    return await api.post('/ImportCalculation', calculation);
  },

  update: async (id: string, calculation: Partial<ImportCalculation>): Promise<void> => {
    await api.put(`/ImportCalculation/${id}`, { ...calculation, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/ImportCalculation/${id}`);
  }
};
