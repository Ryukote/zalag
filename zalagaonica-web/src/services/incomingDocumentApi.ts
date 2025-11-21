import api from './api';

export interface IncomingDocument {
  id: string;
  documentNumber: string;
  supplierName: string;
  documentDate: string;
  totalAmount: number;
  description?: string;
  createdAt: string;
  updatedAt?: string;
}

export const incomingDocumentApi = {
  getAll: async (): Promise<IncomingDocument[]> => {
    const response = await api.get('/IncomingDocument');
    return response.data;
  },

  getById: async (id: string): Promise<IncomingDocument> => {
    const response = await api.get(`/IncomingDocument/${id}`);
    return response.data;
  },

  create: async (document: Partial<IncomingDocument>): Promise<IncomingDocument> => {
    const response = await api.post('/IncomingDocument', document);
    return response.data;
  },

  update: async (id: string, document: Partial<IncomingDocument>): Promise<void> => {
    await api.put(`/IncomingDocument/${id}`, { ...document, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/IncomingDocument/${id}`);
  }
};
