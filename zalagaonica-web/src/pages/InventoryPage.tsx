import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { ArchiveBoxIcon, PlusIcon, DocumentArrowDownIcon, EyeIcon, CheckCircleIcon } from '@heroicons/react/24/outline';

interface InventoryItem {
  id: string;
  inventoryNumber: string;
  date: string;
  articleCode: string;
  articleName: string;
  bookQuantity: number;
  physicalQuantity: number;
  difference: number;
  warehouse: string;
  status: 'pending' | 'verified' | 'approved';
}

export const InventoryPage: React.FC = () => {
  const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>([
    {
      id: '1',
      inventoryNumber: 'INV-2025-001',
      date: '2025-01-15',
      articleCode: 'ART-001',
      articleName: 'Zlatna narukvica 14K',
      bookQuantity: 25,
      physicalQuantity: 24,
      difference: -1,
      warehouse: 'Zalagaonica (ZG3)',
      status: 'pending'
    },
    {
      id: '2',
      inventoryNumber: 'INV-2025-001',
      date: '2025-01-15',
      articleCode: 'ART-002',
      articleName: 'Srebrna ogrlica',
      bookQuantity: 50,
      physicalQuantity: 52,
      difference: 2,
      warehouse: 'Zalagaonica (ZG3)',
      status: 'verified'
    }
  ]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'pending': return 'bg-yellow-100 text-yellow-800';
      case 'verified': return 'bg-blue-100 text-blue-800';
      case 'approved': return 'bg-green-100 text-green-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'pending': return 'Na čekanju';
      case 'verified': return 'Provjereno';
      case 'approved': return 'Odobreno';
      default: return status;
    }
  };

  const getDifferenceColor = (diff: number) => {
    if (diff < 0) return 'text-red-600';
    if (diff > 0) return 'text-green-600';
    return 'text-gray-600';
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <ArchiveBoxIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Inventura
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Popis i usklađivanje stanja skladišta
            </p>
          </div>
          <div className="flex space-x-2">
            <button className="flex items-center px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700">
              <DocumentArrowDownIcon className="h-5 w-5 mr-2" />
              Izvoz Excel
            </button>
            <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
              <PlusIcon className="h-5 w-5 mr-2" />
              Nova inventura
            </button>
          </div>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Broj inventure
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Datum
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Artikl
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Skladište
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Knjižno stanje
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Fizičko stanje
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Razlika
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {inventoryItems.map((item) => (
                <tr key={item.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {item.inventoryNumber}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(item.date).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    <div className="font-medium">{item.articleCode}</div>
                    <div className="text-xs text-gray-400">{item.articleName}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {item.warehouse}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {item.bookQuantity} kom
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {item.physicalQuantity} kom
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className={`font-bold ${getDifferenceColor(item.difference)}`}>
                      {item.difference > 0 ? '+' : ''}{item.difference} kom
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusColor(item.status)}`}>
                      {getStatusText(item.status)}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center items-center space-x-2">
                      <button className="text-indigo-600 hover:text-indigo-900" title="Pregledaj">
                        <EyeIcon className="h-5 w-5" />
                      </button>
                      {item.status !== 'approved' && (
                        <button className="text-green-600 hover:text-green-900" title="Odobri">
                          <CheckCircleIcon className="h-5 w-5" />
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
              {inventoryItems.length === 0 && (
                <tr>
                  <td colSpan={9} className="text-center py-8 text-gray-500">
                    Nema zapisa o inventuri.
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

export default InventoryPage;
