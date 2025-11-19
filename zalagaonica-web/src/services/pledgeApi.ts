import { api } from './api';

export interface Pledge {
  id: string;
  clientId: string;
  clientName: string;
  itemName: string;
  itemDescription: string;
  estimatedValue: number;
  loanAmount: number;
  returnAmount: number;
  period: number;
  pledgeDate: string;
  redeemDeadline: string;
  redeemed: boolean;
  forfeited: boolean;
  itemImagesJson: string;
  warrantyFilesJson: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreatePledgeDto {
  clientId: string;
  clientName: string;
  itemName: string;
  itemDescription: string;
  estimatedValue: number;
  loanAmount: number;
  returnAmount: number;
  period: number;
  pledgeDate: string;
  redeemDeadline: string;
  itemImagesJson?: string;
  warrantyFilesJson?: string;
}

export interface UpdatePledgeDto extends CreatePledgeDto {
  id: string;
}

export const pledgeApi = {
  getAll: () => api.get<Pledge[]>('/Pledge'),

  getById: (id: string) => api.get<Pledge>(`/Pledge/${id}`),

  create: (pledge: CreatePledgeDto) =>
    api.post<Pledge>('/Pledge', pledge),

  update: (id: string, pledge: UpdatePledgeDto) =>
    api.put<void>(`/Pledge/${id}`, pledge),

  delete: (id: string) =>
    api.delete(`/Pledge/${id}`),

  redeem: (id: string) =>
    api.post<{ message: string }>(`/Pledge/${id}/redeem`, {}),

  forfeit: (id: string) =>
    api.post<{ message: string }>(`/Pledge/${id}/forfeit`, {}),
};
