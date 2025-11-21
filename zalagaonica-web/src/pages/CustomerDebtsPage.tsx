import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { UsersIcon, PlusIcon, EyeIcon, BanknotesIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { customerDebtApi, CustomerDebt } from '../services/customerDebtApi';

export const CustomerDebtsPage: React.FC = () => {
  const [debts, setDebts] = useState<CustomerDebt[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    loadDebts();
  }, []);

  const loadDebts = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await customerDebtApi.getAll();
      setDebts(data);
    } catch (err: any) {
      console.error('Error loading debts:', err);
      setError(err.message || 'Greška pri učitavanju zaduženja');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Jeste li sigurni da želite obrisati ovo zaduženje?')) return;

    try {
      await customerDebtApi.delete(id);
      setDebts(debts.filter(d => d.id !== id));
    } catch (err: any) {
      alert('Greška pri brisanju: ' + (err.message || 'Nepoznata greška'));
    }
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'current': case 'ažurno': return 'bg-green-100 text-green-800';
      case 'overdue': case 'kašnjenje': return 'bg-yellow-100 text-yellow-800';
      case 'critical': case 'kritično': return 'bg-red-100 text-red-800';
      case 'paid': case 'plaćeno': return 'bg-blue-100 text-blue-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    const statusMap: { [key: string]: string } = {
      'current': 'Ažurno',
      'overdue': 'Kašnjenje',
      'critical': 'Kritično',
      'paid': 'Plaćeno'
    };
    return statusMap[status.toLowerCase()] || status;
  };

  const calculateDaysPastDue = (dueDate: string) => {
    const due = new Date(dueDate);
    const today = new Date();
    const diffTime = today.getTime() - due.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays > 0 ? diffDays : 0;
  };

  const totalDebtSum = debts.reduce((sum, debt) => sum + (debt.remaining || 0), 0);

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje zaduženja..." />
      </AppLayout>
    );
  }

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
          <button
            onClick={() => setShowModal(true)}
            className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
          >
            <PlusIcon className="h-5 w-5 mr-2" />
            Novo zaduženje
          </button>
        </div>

        {/* Error Message */}
        {error && (
          <div className="bg-red-50 border-l-4 border-red-400 p-4 mb-6">
            <div className="flex">
              <ExclamationCircleIcon className="h-5 w-5 text-red-400 mr-2" />
              <div>
                <p className="text-sm text-red-700">{error}</p>
                <button
                  onClick={loadDebts}
                  className="text-sm text-red-600 underline mt-1"
                >
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        {/* Summary Card */}
        <div className="bg-gradient-to-r from-red-500 to-orange-600 rounded-lg shadow-lg p-6 mb-6 text-white">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <div className="text-sm opacity-90">Ukupno kupaca s dugom</div>
              <div className="text-3xl font-bold">{debts.length}</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Ukupna zaduženja</div>
              <div className="text-3xl font-bold">{totalDebtSum.toFixed(2)} €</div>
            </div>
            <div>
              <div className="text-sm opacity-90">Prosječno zaduženje</div>
              <div className="text-3xl font-bold">
                {debts.length > 0 ? (totalDebtSum / debts.length).toFixed(2) : '0.00'} €
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
              {debts.map((debt) => {
                const daysPastDue = calculateDaysPastDue(debt.dueDate);
                return (
                  <tr key={debt.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 text-sm text-gray-900">
                      <div className="font-medium">{debt.customerName}</div>
                      <div className="text-xs text-gray-400">{debt.customerCode}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {debt.totalDebt.toFixed(2)} €
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-green-600 font-medium">
                      {debt.paid.toFixed(2)} €
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-bold">
                      {debt.remaining.toFixed(2)} €
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      <div>{new Date(debt.dueDate).toLocaleDateString('hr-HR')}</div>
                      {daysPastDue > 0 && (
                        <div className="text-xs text-red-600">Kašnjenje: {daysPastDue} dana</div>
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
                );
              })}
              {debts.length === 0 && !error && (
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
