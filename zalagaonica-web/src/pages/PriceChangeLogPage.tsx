import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { PencilSquareIcon, PlusIcon, EyeIcon } from '@heroicons/react/24/outline';

interface PriceChangeLog {
  id: string;
  documentNumber: string;
  changeDate: string;
  articleCode: string;
  articleName: string;
  oldPrice: number;
  newPrice: number;
  reason: string;
  changedBy: string;
}

export const PriceChangeLogPage: React.FC = () => {
  const [logs, setLogs] = useState<PriceChangeLog[]>([
    {
      id: '1',
      documentNumber: 'ZPC-2025-001',
      changeDate: '2025-01-15',
      articleCode: 'ART-001',
      articleName: 'Zlatna narukvica 14K',
      oldPrice: 2300,
      newPrice: 2500,
      reason: 'Povećanje cijene zlata na tržištu',
      changedBy: 'Marko Marić'
    }
  ]);

  const getPriceChange = (oldPrice: number, newPrice: number) => {
    const change = ((newPrice - oldPrice) / oldPrice) * 100;
    const isIncrease = change > 0;
    return {
      percentage: Math.abs(change).toFixed(2),
      isIncrease,
      color: isIncrease ? 'text-green-600' : 'text-red-600',
      arrow: isIncrease ? '↑' : '↓'
    };
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <PencilSquareIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Zapisnik o promjeni cijene
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Evidencija svih promjena cijena artikala
            </p>
          </div>
          <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
            <PlusIcon className="h-5 w-5 mr-2" />
            Nova promjena cijene
          </button>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Broj zapisnika
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Datum
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Artikl
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Stara cijena
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Nova cijena
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Promjena
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Izvršio
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {logs.map((log) => {
                const change = getPriceChange(log.oldPrice, log.newPrice);
                return (
                  <tr key={log.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      {log.documentNumber}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {new Date(log.changeDate).toLocaleDateString('hr-HR')}
                    </td>
                    <td className="px-6 py-4 text-sm text-gray-500">
                      <div className="font-medium">{log.articleCode}</div>
                      <div className="text-xs text-gray-400">{log.articleName}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {log.oldPrice.toFixed(2)} HRK
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      {log.newPrice.toFixed(2)} HRK
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm">
                      <span className={`font-semibold ${change.color}`}>
                        {change.arrow} {change.percentage}%
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {log.changedBy}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                      <button className="text-indigo-600 hover:text-indigo-900" title="Pregledaj">
                        <EyeIcon className="h-5 w-5" />
                      </button>
                    </td>
                  </tr>
                );
              })}
              {logs.length === 0 && (
                <tr>
                  <td colSpan={8} className="text-center py-8 text-gray-500">
                    Nema zapisa o promjenama cijena.
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

export default PriceChangeLogPage;
