import { api } from './api';

export interface Vehicle {
  id: string;
  make: string;
  model: string;
  year: number;
  plateNumber?: string;
  clientId?: string;
  status?: string;
}

export const vehicleApi = {
  getAll: async () => {
    return await api.get<Vehicle[]>('/Vehicle');
  },

  getById: async (id: string) => {
    return await api.get<Vehicle>(`/Vehicle/${id}`);
  },

  create: async (vehicle: Omit<Vehicle, 'id'>) => {
    return await api.post<Vehicle>('/Vehicle', vehicle);
  },

  update: async (id: string, vehicle: Vehicle) => {
    await api.put(`/Vehicle/${id}`, vehicle);
  },

  delete: async (id: string) => {
    await api.delete(`/Vehicle/${id}`);
  },
};
