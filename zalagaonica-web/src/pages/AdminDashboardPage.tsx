import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import {
  ChartBarIcon,
  CurrencyEuroIcon,
  ShoppingCartIcon,
  ArchiveBoxIcon,
  UserGroupIcon,
  ExclamationCircleIcon
} from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { analyticsApi } from '../services/analyticsApi';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
} from 'chart.js';

// Register ChartJS components
ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement
);

interface DashboardStats {
  totalSales: number;
  totalArticles: number;
  totalClients: number;
  activePledges: number;
}

interface SalesChartData {
  labels: string[];
  data: number[];
}

interface TopProduct {
  articleName: string;
  quantitySold: number;
  totalRevenue: number;
}

export const AdminDashboardPage: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [stats, setStats] = useState<DashboardStats>({
    totalSales: 0,
    totalArticles: 0,
    totalClients: 0,
    activePledges: 0
  });
  const [salesData, setSalesData] = useState<SalesChartData>({ labels: [], data: [] });
  const [topProducts, setTopProducts] = useState<TopProduct[]>([]);
  const [monthlyRevenue, setMonthlyRevenue] = useState<{ month: string; revenue: number }[]>([]);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);

      // Load all analytics data in parallel
      const [statsData, salesChartData, topProductsData, revenueData] = await Promise.all([
        analyticsApi.getDashboardStats(),
        analyticsApi.getSalesChartData(),
        analyticsApi.getTopProducts(10),
        analyticsApi.getMonthlyRevenue()
      ]);

      setStats(statsData);

      // Format sales chart data
      setSalesData({
        labels: salesChartData.map((d: any) => new Date(d.date).toLocaleDateString('hr-HR')),
        data: salesChartData.map((d: any) => d.totalAmount)
      });

      // Map top products data to match local interface
      setTopProducts(topProductsData.map((p: any) => ({
        articleName: p.name,
        quantitySold: p.totalSold,
        totalRevenue: p.totalRevenue
      })));

      // Map monthly revenue data
      setMonthlyRevenue(revenueData.map((r: any) => ({
        month: `${r.year}-${String(r.month).padStart(2, '0')}`,
        revenue: r.revenue
      })));

    } catch (err: any) {
      console.error('Error loading dashboard data:', err);
      setError(err.message || 'Greška pri učitavanju podataka za nadzornu ploču');
    } finally {
      setLoading(false);
    }
  };

  const salesChartConfig = {
    labels: salesData.labels,
    datasets: [
      {
        label: 'Prodaja (€)',
        data: salesData.data,
        borderColor: 'rgb(79, 70, 229)',
        backgroundColor: 'rgba(79, 70, 229, 0.1)',
        tension: 0.3,
        fill: true,
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: false,
      },
    },
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  };

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje nadzorne ploče..." />
      </AppLayout>
    );
  }

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {error && (
          <div className="mb-4 bg-red-50 border-l-4 border-red-400 p-4">
            <div className="flex">
              <ExclamationCircleIcon className="h-5 w-5 text-red-400 mr-2" />
              <div>
                <p className="text-sm text-red-700">{error}</p>
                <button onClick={loadDashboardData} className="text-sm text-red-600 underline mt-1">
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-900 flex items-center">
            <ChartBarIcon className="h-8 w-8 mr-3 text-indigo-600" />
            Nadzorna ploča administratora
          </h1>
          <p className="mt-2 text-sm text-gray-600">
            Pregled statistike i analitike poslovanja
          </p>
        </div>

        {/* Statistics Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <div className="bg-white rounded-lg shadow-md p-6 border-l-4 border-green-500">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <CurrencyEuroIcon className="h-8 w-8 text-green-500" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Ukupna prodaja</p>
                <p className="text-2xl font-bold text-gray-900">{stats.totalSales.toFixed(2)} €</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border-l-4 border-blue-500">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <ArchiveBoxIcon className="h-8 w-8 text-blue-500" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Ukupno artikala</p>
                <p className="text-2xl font-bold text-gray-900">{stats.totalArticles}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border-l-4 border-purple-500">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <UserGroupIcon className="h-8 w-8 text-purple-500" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Ukupno klijenata</p>
                <p className="text-2xl font-bold text-gray-900">{stats.totalClients}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-md p-6 border-l-4 border-yellow-500">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <ShoppingCartIcon className="h-8 w-8 text-yellow-500" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Aktivni zalozi</p>
                <p className="text-2xl font-bold text-gray-900">{stats.activePledges}</p>
              </div>
            </div>
          </div>
        </div>

        {/* Sales Chart */}
        <div className="bg-white rounded-lg shadow-md p-6 mb-8">
          <h2 className="text-xl font-bold text-gray-900 mb-4">Prodaja zadnjih 30 dana</h2>
          <div className="h-80">
            <Line data={salesChartConfig} options={chartOptions} />
          </div>
        </div>

        {/* Two Column Layout */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
          {/* Top Products */}
          <div className="bg-white rounded-lg shadow-md p-6">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Najprodavaniji artikli</h2>
            <div className="space-y-3">
              {topProducts.length === 0 ? (
                <p className="text-gray-500 text-center py-4">Nema podataka o prodaji</p>
              ) : (
                topProducts.slice(0, 5).map((product, index) => (
                  <div key={index} className="flex items-center justify-between border-b pb-3">
                    <div className="flex-1">
                      <p className="font-medium text-gray-900">{product.articleName}</p>
                      <p className="text-sm text-gray-500">{product.quantitySold} kom prodano</p>
                    </div>
                    <div className="text-right">
                      <p className="font-bold text-green-600">{product.totalRevenue.toFixed(2)} €</p>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>

          {/* Monthly Revenue */}
          <div className="bg-white rounded-lg shadow-md p-6">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Mjesečni prihod</h2>
            <div className="space-y-3">
              {monthlyRevenue.length === 0 ? (
                <p className="text-gray-500 text-center py-4">Nema podataka o prihodima</p>
              ) : (
                monthlyRevenue.slice(0, 6).map((item, index) => (
                  <div key={index} className="flex items-center justify-between border-b pb-3">
                    <div>
                      <p className="font-medium text-gray-900">{item.month}</p>
                    </div>
                    <div>
                      <p className="font-bold text-indigo-600">{item.revenue.toFixed(2)} €</p>
                    </div>
                  </div>
                ))
              )}
            </div>
          </div>
        </div>

        {/* Quick Actions */}
        <div className="bg-gradient-to-r from-indigo-500 to-purple-600 rounded-lg shadow-lg p-6 text-white">
          <h2 className="text-xl font-bold mb-4">Brze radnje</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <button className="bg-white bg-opacity-20 hover:bg-opacity-30 rounded-lg p-4 text-left transition-all">
              <p className="font-semibold">Generiraj izvještaj</p>
              <p className="text-sm opacity-90 mt-1">Kreiraj PDF izvještaj za odabrano razdoblje</p>
            </button>
            <button className="bg-white bg-opacity-20 hover:bg-opacity-30 rounded-lg p-4 text-left transition-all">
              <p className="font-semibold">Upravljaj korisnicima</p>
              <p className="text-sm opacity-90 mt-1">Dodaj, uredi ili obriši korisnike sustava</p>
            </button>
            <button className="bg-white bg-opacity-20 hover:bg-opacity-30 rounded-lg p-4 text-left transition-all">
              <p className="font-semibold">Postavke sustava</p>
              <p className="text-sm opacity-90 mt-1">Konfiguriraj postavke zalagaonice</p>
            </button>
          </div>
        </div>
      </div>
    </AppLayout>
  );
};

export default AdminDashboardPage;
