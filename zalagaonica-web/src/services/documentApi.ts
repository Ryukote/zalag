import { api } from './api';

export interface IncomingDocument {
  id: string;
  documentNumber: string;
  supplierName?: string;
  dateReceived: string;
  description?: string;
}

export interface OutputDocumentItem {
  id: string;
  documentId: string;
  articleId: string;
  quantity: number;
  price: number;
  total: number;
}

export const incomingDocumentApi = {
  getAll: () => api.get<IncomingDocument[]>('/IncomingDocument'),

  getById: (id: string) => api.get<IncomingDocument>(`/IncomingDocument/${id}`),

  create: (doc: Omit<IncomingDocument, 'id'>) =>
    api.post<IncomingDocument>('/IncomingDocument', doc),

  update: (id: string, doc: IncomingDocument) =>
    api.put<void>(`/IncomingDocument/${id}`, doc),

  delete: (id: string) =>
    api.delete(`/IncomingDocument/${id}`),
};

export const outputDocumentApi = {
  getAll: () => api.get<OutputDocumentItem[]>('/OutputDocumentItem'),

  getById: (id: string) => api.get<OutputDocumentItem>(`/OutputDocumentItem/${id}`),

  create: (item: Omit<OutputDocumentItem, 'id' | 'total'>) =>
    api.post<OutputDocumentItem>('/OutputDocumentItem', item),

  update: (id: string, item: OutputDocumentItem) =>
    api.put<void>(`/OutputDocumentItem/${id}`, item),

  delete: (id: string) =>
    api.delete(`/OutputDocumentItem/${id}`),
};
