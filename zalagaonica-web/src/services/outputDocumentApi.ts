import api from './api';

export interface OutputDocument {
  id: string;
  clientName: string;
  documentNumber: string;
  documentDate: string;
  totalValue: number;
  status: string;
  documentType: string;
  year: number;
  operator?: string;
  note?: string;
  isPosted: boolean;
  totalWithTax: number;
  pretaxAmount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateOutputDocumentDto {
  clientName: string;
  documentNumber: string;
  documentDate?: string;
  totalValue: number;
  status?: string;
  documentType?: string;
  operator?: string;
  note?: string;
  isPosted?: boolean;
  totalWithTax: number;
  pretaxAmount: number;
}

export interface UpdateOutputDocumentDto {
  id: string;
  clientName: string;
  documentNumber: string;
  documentDate?: string;
  totalValue: number;
  status?: string;
  documentType?: string;
  operator?: string;
  note?: string;
  isPosted?: boolean;
  totalWithTax: number;
  pretaxAmount: number;
}

export const outputDocumentApi = {
  getAll: async (): Promise<OutputDocument[]> => {
    return await api.get('/OutputDocument');
  },

  getById: async (id: string): Promise<OutputDocument> => {
    return await api.get(`/OutputDocument/${id}`);
  },

  create: async (document: CreateOutputDocumentDto): Promise<OutputDocument> => {
    return await api.post('/OutputDocument', document);
  },

  update: async (id: string, document: UpdateOutputDocumentDto): Promise<void> => {
    await api.put(`/OutputDocument/${id}`, document);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/OutputDocument/${id}`);
  }
};
