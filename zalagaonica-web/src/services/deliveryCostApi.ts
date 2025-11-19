import api from './api';
import { DeliveryCost } from '../types/DeliveryCost';

const ENDPOINT = '/DeliveryCost';

export const getAll = async (): Promise<DeliveryCost[]> => {
  const response = await api.get<DeliveryCost[]>(ENDPOINT);
  return response.data;
};

export const getById = async (id: string): Promise<DeliveryCost> => {
  const response = await api.get<DeliveryCost>(`${ENDPOINT}/${id}`);
  return response.data;
};

export const create = async (deliveryCost: Omit<DeliveryCost, 'id'>): Promise<DeliveryCost> => {
  const response = await api.post<DeliveryCost>(ENDPOINT, deliveryCost);
  return response.data;
};

export const update = async (deliveryCost: DeliveryCost): Promise<void> => {
  await api.put(`${ENDPOINT}/${deliveryCost.id}`, deliveryCost);
};

export const remove = async (id: string): Promise<void> => {
  await api.delete(`${ENDPOINT}/${id}`);
};
