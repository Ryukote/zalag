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
    const response = await api.get('/Analytics/dashboard');
    return response.data;
  },

  getSalesChartData: async (startDate?: string, endDate?: string): Promise<SalesChartData[]> => {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);

    const response = await api.get(`/Analytics/sales-chart?${params.toString()}`);
    return response.data;
  },

  getTopProducts: async (count: number = 10): Promise<TopSellingProduct[]> => {
    const response = await api.get(`/Analytics/top-products?count=${count}`);
    return response.data;
  },

  getWarehouseStats: async (): Promise<WarehouseStats[]> => {
    const response = await api.get('/Analytics/warehouse-stats');
    return response.data;
  },

  getPledgeStats: async (): Promise<PledgeStats[]> => {
    const response = await api.get('/Analytics/pledge-stats');
    return response.data;
  },

  getMonthlyRevenue: async (months: number = 12): Promise<MonthlyRevenue[]> => {
    const response = await api.get(`/Analytics/monthly-revenue?months=${months}`);
    return response.data;
  },

  getTopClients: async (count: number = 10): Promise<ClientStats[]> => {
    const response = await api.get(`/Analytics/top-clients?count=${count}`);
    return response.data;
  }
};
