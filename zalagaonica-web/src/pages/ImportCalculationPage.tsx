import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { CalculatorIcon, PlusIcon, PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

interface ImportCalculation {
  id: string;
  documentNumber: string;
  documentDate: string;
  supplierName: string;
  totalAmount: number;
  currency: string;
  exchangeRate: number;
  totalInLocalCurrency: number;
  status: 'draft' | 'confirmed' | 'closed';
}

export const ImportCalculationPage: React.FC = () => {
  const [calculations, setCalculations] = useState<ImportCalculation[]>([
    {
      id: '1',
      documentNumber: 'IMP-2025-001',
      documentDate: '2025-01-15',
      supplierName: 'Dobavljač d.o.o.',
      totalAmount: 5000,
      currency: 'EUR',
      exchangeRate: 7.53,
      totalInLocalCurrency: 37650,
      status: 'confirmed'
    }
  ]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'draft': return 'bg-gray-100 text-gray-800';
      case 'confirmed': return 'bg-green-100 text-green-800';
      case 'closed': return 'bg-blue-100 text-blue-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'draft': return 'Nacrt';
      case 'confirmed': return 'Potvrđeno';
      case 'closed': return 'Zatvoreno';
      default: return status;
    }
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <CalculatorIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Ulazna kalkulacija uvoza
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Upravljanje kalkulacijama uvoza robe iz inozemstva
            </p>
          </div>
          <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
            <PlusIcon className="h-5 w-5 mr-2" />
            Nova kalkulacija
          </button>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Broj dokumenta
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Datum
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Dobavljač
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Iznos
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Tečaj
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Ukupno (HRK)
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
              {calculations.map((calc) => (
                <tr key={calc.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {calc.documentNumber}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(calc.documentDate).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {calc.supplierName}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {calc.totalAmount.toFixed(2)} {calc.currency}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {calc.exchangeRate.toFixed(4)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {calc.totalInLocalCurrency.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusColor(calc.status)}`}>
                      {getStatusText(calc.status)}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center items-center space-x-2">
                      <button className="text-indigo-600 hover:text-indigo-900" title="Uredi">
                        <PencilSquareIcon className="h-5 w-5" />
                      </button>
                      <button className="text-red-600 hover:text-red-900" title="Obriši">
                        <TrashIcon className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {calculations.length === 0 && (
                <tr>
                  <td colSpan={8} className="text-center py-8 text-gray-500">
                    Nema zapisa o kalkulacijama uvoza.
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

export default ImportCalculationPage;
