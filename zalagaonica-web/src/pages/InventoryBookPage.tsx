import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { BookOpenIcon, PlusIcon, PrinterIcon, EyeIcon } from '@heroicons/react/24/outline';

interface InventoryBookEntry {
  id: string;
  date: string;
  entryNumber: string;
  articleCode: string;
  articleName: string;
  quantitySold: number;
  salePrice: number;
  totalSale: number;
  cashier: string;
}

export const InventoryBookPage: React.FC = () => {
  const [entries, setEntries] = useState<InventoryBookEntry[]>([
    {
      id: '1',
      date: '2025-01-15',
      entryNumber: 'KB-001',
      articleCode: 'ART-001',
      articleName: 'Zlatna narukvica 14K',
      quantitySold: 1,
      salePrice: 2500,
      totalSale: 2500,
      cashier: 'Ana Anić'
    },
    {
      id: '2',
      date: '2025-01-15',
      entryNumber: 'KB-002',
      articleCode: 'ART-002',
      articleName: 'Srebrna ogrlica',
      quantitySold: 2,
      salePrice: 450,
      totalSale: 900,
      cashier: 'Ana Anić'
    }
  ]);

  const totalSales = entries.reduce((sum, entry) => sum + entry.totalSale, 0);

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <BookOpenIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Knjiga popisa (Dnevnik maloprodaje)
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Evidencija maloprodajnog prometa
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
              <div className="text-sm opacity-90">Ukupno prodaja danas</div>
              <div className="text-3xl font-bold">{entries.length}</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Ukupan promet</div>
              <div className="text-3xl font-bold">{totalSales.toFixed(2)} HRK</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Prosječna prodaja</div>
              <div className="text-3xl font-bold">
                {entries.length > 0 ? (totalSales / entries.length).toFixed(2) : '0.00'} HRK
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Broj unosa
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Datum
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Artikl
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Količina
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Cijena
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Ukupno
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Blagajnik
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {entries.map((entry) => (
                <tr key={entry.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {entry.entryNumber}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(entry.date).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    <div className="font-medium">{entry.articleCode}</div>
                    <div className="text-xs text-gray-400">{entry.articleName}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {entry.quantitySold} kom
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {entry.salePrice.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {entry.totalSale.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {entry.cashier}
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
                  <td colSpan={8} className="text-center py-8 text-gray-500">
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
