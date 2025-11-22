import api from './api';

export interface DocumentSearchQuery {
  clientName?: string;
  clientOib?: string;
  articleName?: string;
  dateFrom?: string;
  dateTo?: string;
  includeSales?: boolean;
  includePurchases?: boolean;
  includePledges?: boolean;
  includePurchaseRecords?: boolean;
  includeOutputDocuments?: boolean;
}

export interface DocumentResult {
  id: string;
  type: string;
  typeDisplay: string;
  documentNumber: string;
  date: string;
  clientName: string;
  articleName: string;
  amount: number;
  status: string;
}

export interface UnifiedSearchResult {
  totalResults: number;
  sales: DocumentResult[];
  purchases: DocumentResult[];
  pledges: DocumentResult[];
  purchaseRecords: DocumentResult[];
  outputDocuments: DocumentResult[];
}

export interface QuickSearchResult {
  id: string;
  type: string;
  title: string;
  subtitle: string;
  icon: string;
}

export interface SearchStats {
  lastMonthSales: number;
  lastMonthPurchases: number;
  lastMonthPledges: number;
  lastMonthPurchaseRecords: number;
  lastMonthOutputDocuments: number;
  total: number;
}

export const unifiedSearchApi = {
  search: async (query: DocumentSearchQuery): Promise<UnifiedSearchResult> => {
    return await api.post('/UnifiedSearch/search', query);
  },

  quickSearch: async (searchTerm: string): Promise<QuickSearchResult[]> => {
    return await api.get(`/UnifiedSearch/quick?q=${encodeURIComponent(searchTerm)}`);
  },

  getStats: async (): Promise<SearchStats> => {
    return await api.get('/UnifiedSearch/stats');
  }
};
