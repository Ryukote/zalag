import api from './api';
import { Customer } from '../types/Customer';

const ENDPOINT = '/Customer';

export const getAll = async (): Promise<Customer[]> => {
  const response = await api.get<Customer[]>(ENDPOINT);
  return response.data;
};

export const getById = async (id: string): Promise<Customer> => {
  const response = await api.get<Customer>(`${ENDPOINT}/${id}`);
  return response.data;
};

export const create = async (customer: Omit<Customer, 'id' | 'createdAt' | 'updatedAt'>): Promise<Customer> => {
  const response = await api.post<Customer>(ENDPOINT, customer);
  return response.data;
};

export const update = async (customer: Customer): Promise<void> => {
  await api.put(`${ENDPOINT}/${customer.id}`, customer);
};

export const remove = async (id: string): Promise<void> => {
  await api.delete(`${ENDPOINT}/${id}`);
};
