import { api } from './api';
import { Article } from '../types/article';

export type { Article };

export const articleApi = {
  getAll: async () => {
    const response = await api.get<Article[]>('/Article');
    return response.data;
  },

  getById: async (id: string) => {
    const response = await api.get<Article>(`/Article/${id}`);
    return response.data;
  },

  create: async (article: Omit<Article, 'id' | 'createdAt' | 'updatedAt'>) => {
    const response = await api.post<Article>('/Article', article);
    return response.data;
  },

  update: async (id: string, article: Article) => {
    await api.put(`/Article/${id}`, article);
  },

  delete: async (id: string) => {
    await api.delete(`/Article/${id}`);
  },
};
