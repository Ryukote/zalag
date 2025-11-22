import api from './api';

export interface DashboardStats {
  totalSales: number;
  totalArticles: number;
  totalClients: number;
  activePledges: number;
  todaySalesAmount: number;
  monthSalesAmount: number;
  yearSalesAmount: number;
}

export interface SalesChartData {
  date: string;
  totalAmount: number;
  count: number;
}

export interface TopSellingProduct {
  articleId: string;
  name: string;
  totalSold: number;
  totalRevenue: number;
}

export interface WarehouseStats {
  warehouseId: string;
  warehouseName: string;
  totalArticles: number;
  totalValue: number;
}

export interface PledgeStats {
  status: string;
  count: number;
}

export interface MonthlyRevenue {
  year: number;
  month: number;
  revenue: number;
  salesCount: number;
}

export interface ClientStats {
  clientId: string;
  clientName: string;
  totalPurchases: number;
  totalPledges: number;
  totalSpent: number;
}

export const analyticsApi = {
  getDashboardStats: async (): Promise<DashboardStats> => {
    return await api.get<DashboardStats>('/Analytics/dashboard');
  },

  getSalesChartData: async (startDate?: string, endDate?: string): Promise<SalesChartData[]> => {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);

    return await api.get<SalesChartData[]>(`/Analytics/sales-chart?${params.toString()}`);
  },

  getTopProducts: async (count: number = 10): Promise<TopSellingProduct[]> => {
    return await api.get<TopSellingProduct[]>(`/Analytics/top-products?count=${count}`);
  },

  getWarehouseStats: async (): Promise<WarehouseStats[]> => {
    return await api.get<WarehouseStats[]>('/Analytics/warehouse-stats');
  },

  getPledgeStats: async (): Promise<PledgeStats[]> => {
    return await api.get<PledgeStats[]>('/Analytics/pledge-stats');
  },

  getMonthlyRevenue: async (months: number = 12): Promise<MonthlyRevenue[]> => {
    return await api.get<MonthlyRevenue[]>(`/Analytics/monthly-revenue?months=${months}`);
  },

  getTopClients: async (count: number = 10): Promise<ClientStats[]> => {
    return await api.get<ClientStats[]>(`/Analytics/top-clients?count=${count}`);
  }
};
