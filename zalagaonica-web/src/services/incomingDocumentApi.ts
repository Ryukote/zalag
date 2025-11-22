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
    return await api.get('/IncomingDocument');
  },

  getById: async (id: string): Promise<IncomingDocument> => {
    return await api.get(`/IncomingDocument/${id}`);
  },

  create: async (document: Partial<IncomingDocument>): Promise<IncomingDocument> => {
    return await api.post('/IncomingDocument', document);
  },

  update: async (id: string, document: Partial<IncomingDocument>): Promise<void> => {
    await api.put(`/IncomingDocument/${id}`, { ...document, id });
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/IncomingDocument/${id}`);
  }
};
