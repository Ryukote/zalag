import api from './api';
import { Customer } from '../types/Customer';

const ENDPOINT = '/Customer';

export const getAll = async (): Promise<Customer[]> => {
  return await api.get<Customer[]>(ENDPOINT);
};

export const getById = async (id: string): Promise<Customer> => {
  return await api.get<Customer>(`${ENDPOINT}/${id}`);
};

export const create = async (customer: Omit<Customer, 'id' | 'createdAt' | 'updatedAt'>): Promise<Customer> => {
  return await api.post<Customer>(ENDPOINT, customer);
};

export const update = async (customer: Customer): Promise<void> => {
  await api.put(`${ENDPOINT}/${customer.id}`, customer);
};

export const remove = async (id: string): Promise<void> => {
  await api.delete(`${ENDPOINT}/${id}`);
};
