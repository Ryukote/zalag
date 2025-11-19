import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { UsersIcon, PlusIcon, EyeIcon, BanknotesIcon } from '@heroicons/react/24/outline';

interface CustomerDebt {
  id: string;
  customerName: string;
  customerCode: string;
  totalDebt: number;
  paid: number;
  remaining: number;
  dueDate: string;
  daysPastDue: number;
  status: 'current' | 'overdue' | 'critical';
}

export const CustomerDebtsPage: React.FC = () => {
  const [debts, setDebts] = useState<CustomerDebt[]>([
    {
      id: '1',
      customerName: 'Ivan Horvat',
      customerCode: 'KOM-001',
      totalDebt: 15000,
      paid: 5000,
      remaining: 10000,
      dueDate: '2025-02-01',
      daysPastDue: 0,
      status: 'current'
    },
    {
      id: '2',
      customerName: 'Marko Marić',
      customerCode: 'KOM-002',
      totalDebt: 8000,
      paid: 2000,
      remaining: 6000,
      dueDate: '2025-01-10',
      daysPastDue: 5,
      status: 'overdue'
    }
  ]);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'current': return 'bg-green-100 text-green-800';
      case 'overdue': return 'bg-yellow-100 text-yellow-800';
      case 'critical': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'current': return 'Ažurno';
      case 'overdue': return 'Kašnjenje';
      case 'critical': return 'Kritično';
      default: return status;
    }
  };

  const totalDebtSum = debts.reduce((sum, debt) => sum + debt.remaining, 0);

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <UsersIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Kupci - zaduženja
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Pregled i upravljanje zaduženjima kupaca
            </p>
          </div>
          <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
            <PlusIcon className="h-5 w-5 mr-2" />
            Novo zaduženje
          </button>
        </div>

        {/* Summary Card */}
        <div className="bg-gradient-to-r from-red-500 to-orange-600 rounded-lg shadow-lg p-6 mb-6 text-white">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <div className="text-sm opacity-90">Ukupno kupaca s dugom</div>
              <div className="text-3xl font-bold">{debts.length}</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Ukupna zaduženja</div>
              <div className="text-3xl font-bold">{totalDebtSum.toFixed(2)} HRK</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Prosječno zaduženje</div>
              <div className="text-3xl font-bold">
                {debts.length > 0 ? (totalDebtSum / debts.length).toFixed(2) : '0.00'} HRK
              </div>
            </div>
          </div>
        </div>

        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Kupac
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Ukupan dug
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Plaćeno
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Preostalo
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Rok plaćanja
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
              {debts.map((debt) => (
                <tr key={debt.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 text-sm text-gray-900">
                    <div className="font-medium">{debt.customerName}</div>
                    <div className="text-xs text-gray-400">{debt.customerCode}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {debt.totalDebt.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-green-600 font-medium">
                    {debt.paid.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-bold">
                    {debt.remaining.toFixed(2)} HRK
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    <div>{new Date(debt.dueDate).toLocaleDateString('hr-HR')}</div>
                    {debt.daysPastDue > 0 && (
                      <div className="text-xs text-red-600">Kašnjenje: {debt.daysPastDue} dana</div>
                    )}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusColor(debt.status)}`}>
                      {getStatusText(debt.status)}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center items-center space-x-2">
                      <button className="text-indigo-600 hover:text-indigo-900" title="Detalji">
                        <EyeIcon className="h-5 w-5" />
                      </button>
                      <button className="text-green-600 hover:text-green-900" title="Naplati">
                        <BanknotesIcon className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {debts.length === 0 && (
                <tr>
                  <td colSpan={7} className="text-center py-8 text-gray-500">
                    Nema zapisa o zaduženjima kupaca.
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

export default CustomerDebtsPage;
