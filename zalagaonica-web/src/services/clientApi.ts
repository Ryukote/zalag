import { api } from './api';

export interface Client {
  id: string;
  name: string;
  address?: string;
  city?: string;
  postalCode?: string;
  country?: string;
  phone?: string;
  email?: string;
  oib?: string;
  idCardNumber?: string;
  accountNumber?: string;
  iban?: string;
  type: 'legal' | 'individual';
  isActive?: boolean;
  notes?: string;
  status: 'active' | 'inactive';
  createdAt: string;
  updatedAt: string;
}

export const clientApi = {
  getAll: () => api.get<Client[]>('/Client'),

  getById: (id: string) => api.get<Client>(`/Client/${id}`),

  create: (client: Omit<Client, 'id' | 'createdAt' | 'updatedAt'>) =>
    api.post<Client>('/Client', client),

  update: (id: string, client: Client) =>
    api.put<void>(`/Client/${id}`, client),

  delete: (id: string) =>
    api.delete(`/Client/${id}`),
};
