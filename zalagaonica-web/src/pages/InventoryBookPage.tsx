import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { BookOpenIcon, PlusIcon, PrinterIcon, EyeIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { inventoryBookApi, InventoryBook } from '../services/inventoryBookApi';

export const InventoryBookPage: React.FC = () => {
  const [entries, setEntries] = useState<InventoryBook[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadEntries();
  }, []);

  const loadEntries = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await inventoryBookApi.getAll();
      setEntries(data);
    } catch (err: any) {
      console.error('Error loading inventory book entries:', err);
      setError(err.message || 'Greška pri učitavanju knjige popisa');
    } finally {
      setLoading(false);
    }
  };

  const totalIn = entries.reduce((sum, entry) => sum + entry.inQuantity, 0);
  const totalOut = entries.reduce((sum, entry) => sum + entry.outQuantity, 0);

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje knjige popisa..." />
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
                <button onClick={loadEntries} className="text-sm text-red-600 underline mt-1">
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <BookOpenIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Knjiga popisa (KPO)
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Evidencija kretanja artikala u skladištu
            </p>
          </div>
          <div className="flex space-x-2">
            <button className="flex items-center px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700">
              <PrinterIcon className="h-5 w-5 mr-2" />
              Ispiši izvješće
            </button>
            <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
              <PlusIcon className="h-5 w-5 mr-2" />
              Novi unos
            </button>
          </div>
        </div>

        {/* Summary Card */}
        <div className="bg-gradient-to-r from-indigo-500 to-purple-600 rounded-lg shadow-lg p-6 mb-6 text-white">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <div className="text-sm opacity-90">Ukupno zapisa</div>
              <div className="text-3xl font-bold">{entries.length}</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Ukupan ulaz</div>
              <div className="text-3xl font-bold">{totalIn} kom</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Ukupan izlaz</div>
              <div className="text-3xl font-bold">{totalOut} kom</div>
            </div>
          </div>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Datum
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Artikl
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Dokument
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Ulaz
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Izlaz
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Stanje
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {entries.map((entry) => (
                <tr key={entry.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(entry.date).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-900">
                    <div className="font-medium">{entry.articleName}</div>
                    {entry.notes && <div className="text-xs text-gray-400">{entry.notes}</div>}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {entry.documentNumber}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600 font-medium">
                    {entry.inQuantity > 0 ? `+${entry.inQuantity}` : ''}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600 font-medium">
                    {entry.outQuantity > 0 ? `-${entry.outQuantity}` : ''}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right font-semibold text-gray-900">
                    {entry.balance}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <button className="text-indigo-600 hover:text-indigo-900" title="Pregledaj">
                      <EyeIcon className="h-5 w-5" />
                    </button>
                  </td>
                </tr>
              ))}
              {entries.length === 0 && (
                <tr>
                  <td colSpan={7} className="text-center py-8 text-gray-500">
                    Nema zapisa u knjizi popisa.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </AppLayout>
  );
};

export default InventoryBookPage;
