import { api } from './api';

export interface UnitOfMeasure {
  id: string;
  code: string;
  name: string;
}

export const unitOfMeasureApi = {
  getAll: async () => {
    return await api.get<UnitOfMeasure[]>('/UnitOfMeasure');
  },

  getById: async (id: string) => {
    return await api.get<UnitOfMeasure>(`/UnitOfMeasure/${id}`);
  },

  create: async (unit: Omit<UnitOfMeasure, 'id'>) => {
    return await api.post<UnitOfMeasure>('/UnitOfMeasure', unit);
  },

  update: async (id: string, unit: UnitOfMeasure) => {
    await api.put(`/UnitOfMeasure/${id}`, unit);
  },

  delete: async (id: string) => {
    await api.delete(`/UnitOfMeasure/${id}`);
  },
};
