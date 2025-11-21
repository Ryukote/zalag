import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import {
  MagnifyingGlassIcon,
  FunnelIcon,
  DocumentTextIcon,
  ShoppingCartIcon,
  ShoppingBagIcon,
  ArchiveBoxIcon,
  DocumentDuplicateIcon,
  CalendarIcon,
  UserIcon,
  TagIcon,
  XMarkIcon,
  ArrowDownTrayIcon,
  EyeIcon
} from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { unifiedSearchApi, DocumentSearchQuery, DocumentResult, SearchStats } from '../services/unifiedSearchApi';

export const UnifiedDocumentsPage: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState<DocumentSearchQuery>({
    includeSales: true,
    includePurchases: true,
    includePledges: true,
    includePurchaseRecords: true,
    includeOutputDocuments: true
  });

  const [results, setResults] = useState<{
    totalResults: number;
    sales: DocumentResult[];
    purchases: DocumentResult[];
    pledges: DocumentResult[];
    purchaseRecords: DocumentResult[];
    outputDocuments: DocumentResult[];
  } | null>(null);

  const [stats, setStats] = useState<SearchStats | null>(null);
  const [loading, setLoading] = useState(false);
  const [showFilters, setShowFilters] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'all' | 'sales' | 'purchases' | 'pledges' | 'records' | 'output'>('all');

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const data = await unifiedSearchApi.getStats();
      setStats(data);
    } catch (err: any) {
      console.error('Error loading stats:', err);
    }
  };

  const handleSearch = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await unifiedSearchApi.search(searchQuery);
      setResults(data);
    } catch (err: any) {
      console.error('Error searching:', err);
      setError(err.message || 'Greška pri pretraživanju dokumenata');
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setSearchQuery({
      includeSales: true,
      includePurchases: true,
      includePledges: true,
      includePurchaseRecords: true,
      includeOutputDocuments: true
    });
    setResults(null);
    setError(null);
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'Sale': return <ShoppingCartIcon className="h-5 w-5" />;
      case 'Purchase': return <ShoppingBagIcon className="h-5 w-5" />;
      case 'Pledge': return <ArchiveBoxIcon className="h-5 w-5" />;
      case 'PurchaseRecord': return <DocumentDuplicateIcon className="h-5 w-5" />;
      case 'OutputDocument': return <DocumentTextIcon className="h-5 w-5" />;
      default: return <DocumentTextIcon className="h-5 w-5" />;
    }
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'Sale': return 'bg-green-100 text-green-800';
      case 'Purchase': return 'bg-blue-100 text-blue-800';
      case 'Pledge': return 'bg-purple-100 text-purple-800';
      case 'PurchaseRecord': return 'bg-orange-100 text-orange-800';
      case 'OutputDocument': return 'bg-indigo-100 text-indigo-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getAllDocuments = (): DocumentResult[] => {
    if (!results) return [];
    return [
      ...results.sales,
      ...results.purchases,
      ...results.pledges,
      ...results.purchaseRecords,
      ...results.outputDocuments
    ].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  };

  const getFilteredDocuments = (): DocumentResult[] => {
    if (!results) return [];

    switch (activeTab) {
      case 'sales': return results.sales;
      case 'purchases': return results.purchases;
      case 'pledges': return results.pledges;
      case 'records': return results.purchaseRecords;
      case 'output': return results.outputDocuments;
      case 'all':
      default: return getAllDocuments();
    }
  };

  const DocumentCard: React.FC<{ doc: DocumentResult }> = ({ doc }) => (
    <div className="bg-white border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow">
      <div className="flex items-start justify-between">
        <div className="flex items-start space-x-3 flex-1">
          <div className={`p-2 rounded-lg ${getTypeColor(doc.type)}`}>
            {getTypeIcon(doc.type)}
          </div>
          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-2 mb-1">
              <span className={`px-2 py-1 text-xs font-semibold rounded ${getTypeColor(doc.type)}`}>
                {doc.typeDisplay}
              </span>
              <span className="text-sm font-mono text-gray-500">{doc.documentNumber}</span>
            </div>
            <h3 className="text-sm font-semibold text-gray-900 truncate">{doc.articleName}</h3>
            <div className="flex items-center gap-4 mt-2 text-xs text-gray-600">
              <span className="flex items-center gap-1">
                <UserIcon className="h-3 w-3" />
                {doc.clientName}
              </span>
              <span className="flex items-center gap-1">
                <CalendarIcon className="h-3 w-3" />
                {new Date(doc.date).toLocaleDateString('hr-HR')}
              </span>
            </div>
          </div>
        </div>
        <div className="flex flex-col items-end gap-2">
          <span className="text-lg font-bold text-gray-900">{doc.amount.toFixed(2)} €</span>
          <span className="text-xs px-2 py-1 bg-gray-100 text-gray-600 rounded">{doc.status}</span>
          <button className="text-indigo-600 hover:text-indigo-900 text-sm flex items-center gap-1">
            <EyeIcon className="h-4 w-4" />
            Detalji
          </button>
        </div>
      </div>
    </div>
  );

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 flex items-center">
            <MagnifyingGlassIcon className="h-8 w-8 mr-3 text-indigo-600" />
            Pretraživanje dokumenata
          </h1>
          <p className="mt-2 text-sm text-gray-600">
            Centralizirano pretraživanje svih dokumenata - prodaja, otkup, zaloga i više
          </p>
        </div>

        {/* Stats Cards */}
        {stats && (
          <div className="grid grid-cols-2 md:grid-cols-6 gap-4 mb-6">
            <div className="bg-gradient-to-br from-green-500 to-green-600 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Prodaje (30d)</div>
              <div className="text-2xl font-bold">{stats.lastMonthSales}</div>
            </div>
            <div className="bg-gradient-to-br from-blue-500 to-blue-600 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Otkupi (30d)</div>
              <div className="text-2xl font-bold">{stats.lastMonthPurchases}</div>
            </div>
            <div className="bg-gradient-to-br from-purple-500 to-purple-600 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Zalozi (30d)</div>
              <div className="text-2xl font-bold">{stats.lastMonthPledges}</div>
            </div>
            <div className="bg-gradient-to-br from-orange-500 to-orange-600 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Zapisi (30d)</div>
              <div className="text-2xl font-bold">{stats.lastMonthPurchaseRecords}</div>
            </div>
            <div className="bg-gradient-to-br from-indigo-500 to-indigo-600 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Izlazni dok. (30d)</div>
              <div className="text-2xl font-bold">{stats.lastMonthOutputDocuments}</div>
            </div>
            <div className="bg-gradient-to-br from-gray-700 to-gray-800 text-white rounded-lg p-4">
              <div className="text-xs opacity-90">Ukupno (30d)</div>
              <div className="text-2xl font-bold">{stats.total}</div>
            </div>
          </div>
        )}

        {/* Search Filters */}
        <div className="bg-white rounded-lg shadow-md p-6 mb-6">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold text-gray-900 flex items-center">
              <FunnelIcon className="h-5 w-5 mr-2" />
              Filtri pretraživanja
            </h2>
            <button
              onClick={() => setShowFilters(!showFilters)}
              className="text-sm text-indigo-600 hover:text-indigo-800"
            >
              {showFilters ? 'Sakrij' : 'Prikaži'} filtre
            </button>
          </div>

          {showFilters && (
            <div className="space-y-4">
              {/* Text Filters */}
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Ime klijenta
                  </label>
                  <input
                    type="text"
                    value={searchQuery.clientName || ''}
                    onChange={(e) => setSearchQuery({ ...searchQuery, clientName: e.target.value })}
                    placeholder="Pretraži po imenu klijenta..."
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    OIB klijenta
                  </label>
                  <input
                    type="text"
                    value={searchQuery.clientOib || ''}
                    onChange={(e) => setSearchQuery({ ...searchQuery, clientOib: e.target.value })}
                    placeholder="Pretraži po OIB-u..."
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Naziv artikla
                  </label>
                  <input
                    type="text"
                    value={searchQuery.articleName || ''}
                    onChange={(e) => setSearchQuery({ ...searchQuery, articleName: e.target.value })}
                    placeholder="Pretraži po nazivu artikla..."
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
                  />
                </div>
              </div>

              {/* Date Filters */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Datum od
                  </label>
                  <input
                    type="date"
                    value={searchQuery.dateFrom || ''}
                    onChange={(e) => setSearchQuery({ ...searchQuery, dateFrom: e.target.value })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Datum do
                  </label>
                  <input
                    type="date"
                    value={searchQuery.dateTo || ''}
                    onChange={(e) => setSearchQuery({ ...searchQuery, dateTo: e.target.value })}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
                  />
                </div>
              </div>

              {/* Document Type Checkboxes */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Vrste dokumenata
                </label>
                <div className="flex flex-wrap gap-4">
                  <label className="flex items-center">
                    <input
                      type="checkbox"
                      checked={searchQuery.includeSales}
                      onChange={(e) => setSearchQuery({ ...searchQuery, includeSales: e.target.checked })}
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span className="ml-2 text-sm text-gray-700">Prodaje</span>
                  </label>
                  <label className="flex items-center">
                    <input
                      type="checkbox"
                      checked={searchQuery.includePurchases}
                      onChange={(e) => setSearchQuery({ ...searchQuery, includePurchases: e.target.checked })}
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span className="ml-2 text-sm text-gray-700">Otkupi</span>
                  </label>
                  <label className="flex items-center">
                    <input
                      type="checkbox"
                      checked={searchQuery.includePledges}
                      onChange={(e) => setSearchQuery({ ...searchQuery, includePledges: e.target.checked })}
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span className="ml-2 text-sm text-gray-700">Zalozi</span>
                  </label>
                  <label className="flex items-center">
                    <input
                      type="checkbox"
                      checked={searchQuery.includePurchaseRecords}
                      onChange={(e) => setSearchQuery({ ...searchQuery, includePurchaseRecords: e.target.checked })}
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span className="ml-2 text-sm text-gray-700">Otkupni zapisi</span>
                  </label>
                  <label className="flex items-center">
                    <input
                      type="checkbox"
                      checked={searchQuery.includeOutputDocuments}
                      onChange={(e) => setSearchQuery({ ...searchQuery, includeOutputDocuments: e.target.checked })}
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span className="ml-2 text-sm text-gray-700">Izlazni dokumenti</span>
                  </label>
                </div>
              </div>

              {/* Action Buttons */}
              <div className="flex gap-3 pt-4">
                <button
                  onClick={handleSearch}
                  disabled={loading}
                  className="flex-1 bg-indigo-600 text-white px-6 py-2 rounded-md hover:bg-indigo-700 disabled:bg-gray-400 flex items-center justify-center gap-2"
                >
                  {loading ? <LoadingSpinner size="small" /> : <MagnifyingGlassIcon className="h-5 w-5" />}
                  Pretraži
                </button>
                <button
                  onClick={handleReset}
                  className="px-6 py-2 border border-gray-300 rounded-md hover:bg-gray-50 flex items-center gap-2"
                >
                  <XMarkIcon className="h-5 w-5" />
                  Resetuj
                </button>
                {results && (
                  <button className="px-6 py-2 border border-gray-300 rounded-md hover:bg-gray-50 flex items-center gap-2">
                    <ArrowDownTrayIcon className="h-5 w-5" />
                    Izvezi
                  </button>
                )}
              </div>
            </div>
          )}
        </div>

        {/* Error Message */}
        {error && (
          <div className="bg-red-50 border-l-4 border-red-400 p-4 mb-6">
            <p className="text-sm text-red-700">{error}</p>
          </div>
        )}

        {/* Results */}
        {results && (
          <div className="bg-white rounded-lg shadow-md">
            {/* Tabs */}
            <div className="border-b border-gray-200">
              <nav className="flex -mb-px overflow-x-auto">
                <button
                  onClick={() => setActiveTab('all')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'all'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Sve ({results.totalResults})
                </button>
                <button
                  onClick={() => setActiveTab('sales')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'sales'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Prodaje ({results.sales.length})
                </button>
                <button
                  onClick={() => setActiveTab('purchases')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'purchases'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Otkupi ({results.purchases.length})
                </button>
                <button
                  onClick={() => setActiveTab('pledges')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'pledges'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Zalozi ({results.pledges.length})
                </button>
                <button
                  onClick={() => setActiveTab('records')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'records'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Zapisi ({results.purchaseRecords.length})
                </button>
                <button
                  onClick={() => setActiveTab('output')}
                  className={`px-6 py-4 text-sm font-medium border-b-2 whitespace-nowrap ${
                    activeTab === 'output'
                      ? 'border-indigo-500 text-indigo-600'
                      : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                  }`}
                >
                  Izlazni dok. ({results.outputDocuments.length})
                </button>
              </nav>
            </div>

            {/* Results Grid */}
            <div className="p-6">
              {getFilteredDocuments().length === 0 ? (
                <div className="text-center py-12 text-gray-500">
                  <DocumentTextIcon className="h-16 w-16 mx-auto mb-4 text-gray-300" />
                  <p className="text-lg font-medium">Nema rezultata</p>
                  <p className="text-sm mt-1">Pokušajte promijeniti kriterije pretraživanja</p>
                </div>
              ) : (
                <div className="grid grid-cols-1 gap-4">
                  {getFilteredDocuments().map((doc) => (
                    <DocumentCard key={`${doc.type}-${doc.id}`} doc={doc} />
                  ))}
                </div>
              )}
            </div>
          </div>
        )}

        {/* Initial State */}
        {!results && !loading && (
          <div className="bg-white rounded-lg shadow-md p-12 text-center">
            <MagnifyingGlassIcon className="h-16 w-16 mx-auto mb-4 text-gray-300" />
            <h3 className="text-lg font-medium text-gray-900 mb-2">
              Spremno za pretraživanje
            </h3>
            <p className="text-gray-600">
              Unesite kriterije pretraživanja i kliknite "Pretraži" za prikaz dokumenata
            </p>
          </div>
        )}
      </div>
    </AppLayout>
  );
};

export default UnifiedDocumentsPage;
