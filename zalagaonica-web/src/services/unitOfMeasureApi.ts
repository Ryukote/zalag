import { api } from './api';

export interface UnitOfMeasure {
  id: string;
  code: string;
  name: string;
}

export const unitOfMeasureApi = {
  getAll: async () => {
    const response = await api.get<UnitOfMeasure[]>('/UnitOfMeasure');
    return response.data;
  },

  getById: async (id: string) => {
    const response = await api.get<UnitOfMeasure>(`/UnitOfMeasure/${id}`);
    return response.data;
  },

  create: async (unit: Omit<UnitOfMeasure, 'id'>) => {
    const response = await api.post<UnitOfMeasure>('/UnitOfMeasure', unit);
    return response.data;
  },

  update: async (id: string, unit: UnitOfMeasure) => {
    await api.put(`/UnitOfMeasure/${id}`, unit);
  },

  delete: async (id: string) => {
    await api.delete(`/UnitOfMeasure/${id}`);
  },
};
