import { api } from './api';
import { Article } from '../types/article';

export type { Article };

export const articleApi = {
  getAll: async () => {
    return await api.get<Article[]>('/Article');
  },

  getById: async (id: string) => {
    return await api.get<Article>(`/Article/${id}`);
  },

  create: async (article: Omit<Article, 'id' | 'createdAt' | 'updatedAt'>) => {
    return await api.post<Article>('/Article', article);
  },

  update: async (id: string, article: Article) => {
    await api.put(`/Article/${id}`, article);
  },

  delete: async (id: string) => {
    await api.delete(`/Article/${id}`);
  },
};
