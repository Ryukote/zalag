import { api } from './api';

export interface Warehouse {
  id: string;
  code: string;
  name: string;
  location?: string;
  warehouseTypeId?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface WarehouseType {
  id: string;
  code: string;
  name: string;
  description?: string;
}

export const warehouseApi = {
  getAll: () => api.get<Warehouse[]>('/Warehouse'),

  getById: (id: string) => api.get<Warehouse>(`/Warehouse/${id}`),

  create: (warehouse: Omit<Warehouse, 'id' | 'createdAt' | 'updatedAt'>) =>
    api.post<Warehouse>('/Warehouse', warehouse),

  update: (id: string, warehouse: Warehouse) =>
    api.put<void>(`/Warehouse/${id}`, warehouse),

  delete: (id: string) =>
    api.delete(`/Warehouse/${id}`),
};

export const warehouseTypeApi = {
  getAll: () => api.get<WarehouseType[]>('/WarehouseType'),

  getById: (id: string) => api.get<WarehouseType>(`/WarehouseType/${id}`),

  create: (type: Omit<WarehouseType, 'id'>) =>
    api.post<WarehouseType>('/WarehouseType', type),

  update: (id: string, type: WarehouseType) =>
    api.put<void>(`/WarehouseType/${id}`, type),

  delete: (id: string) =>
    api.delete(`/WarehouseType/${id}`),
};
