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
    const response = await api.get<Vehicle[]>('/Vehicle');
    return response.data;
  },

  getById: async (id: string) => {
    const response = await api.get<Vehicle>(`/Vehicle/${id}`);
    return response.data;
  },

  create: async (vehicle: Omit<Vehicle, 'id'>) => {
    const response = await api.post<Vehicle>('/Vehicle', vehicle);
    return response.data;
  },

  update: async (id: string, vehicle: Vehicle) => {
    await api.put(`/Vehicle/${id}`, vehicle);
  },

  delete: async (id: string) => {
    await api.delete(`/Vehicle/${id}`);
  },
};
