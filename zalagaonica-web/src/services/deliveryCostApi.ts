import api from './api';
import { DeliveryCost } from '../types/DeliveryCost';

const ENDPOINT = '/DeliveryCost';

export const getAll = async (): Promise<DeliveryCost[]> => {
  return await api.get<DeliveryCost[]>(ENDPOINT);
};

export const getById = async (id: string): Promise<DeliveryCost> => {
  return await api.get<DeliveryCost>(`${ENDPOINT}/${id}`);
};

export const create = async (deliveryCost: Omit<DeliveryCost, 'id'>): Promise<DeliveryCost> => {
  return await api.post<DeliveryCost>(ENDPOINT, deliveryCost);
};

export const update = async (deliveryCost: DeliveryCost): Promise<void> => {
  await api.put(`${ENDPOINT}/${deliveryCost.id}`, deliveryCost);
};

export const remove = async (id: string): Promise<void> => {
  await api.delete(`${ENDPOINT}/${id}`);
};
