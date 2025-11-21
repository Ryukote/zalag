import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { PowerIcon, PrinterIcon, CheckCircleIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { dailyClosingApi } from '../services/dailyClosingApi';

export const EndOfWorkPage: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [workDay, setWorkDay] = useState({
    id: '',
    date: new Date().toISOString().split('T')[0],
    cashierName: '',
    startingCash: 0,
    totalSales: 0,
    totalExpenses: 0,
    cashInRegister: 0,
    expectedCash: 0,
    difference: 0,
    isClosed: false
  });

  useEffect(() => {
    loadWorkDay();
  }, []);

  const loadWorkDay = async () => {
    try {
      setLoading(true);
      setError(null);
      const today = new Date().toISOString().split('T')[0];
      const data = await dailyClosingApi.getByDate(today);
      const expectedCash = data.startingCash + data.totalSales - data.totalExpenses;
      const difference = data.cashInRegister - expectedCash;
      setWorkDay({
        id: data.id,
        date: data.date,
        cashierName: data.cashierName,
        startingCash: data.startingCash,
        totalSales: data.totalSales,
        totalExpenses: data.totalExpenses,
        cashInRegister: data.cashInRegister,
        expectedCash,
        difference,
        isClosed: data.isClosed
      });
    } catch (err: any) {
      console.error('Error loading work day:', err);
      setError(err.message || 'Greška pri učitavanju podataka o radnom danu');
    } finally {
      setLoading(false);
    }
  };

  const handleCloseDay = async () => {
    if (!window.confirm('Jeste li sigurni da želite zatvoriti radni dan? Ova akcija se ne može poništiti.')) {
      return;
    }

    try {
      await dailyClosingApi.update(workDay.id, { isClosed: true, closedAt: new Date().toISOString() });
      setWorkDay(prev => ({ ...prev, isClosed: true }));
      alert('Radni dan uspješno zatvoren!');
    } catch (err: any) {
      alert('Greška pri zatvaranju radnog dana: ' + (err.message || 'Nepoznata greška'));
    }
  };

  const handlePrintReport = () => {
    alert('Ispis izvještaja o kraju rada...');
  };

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje podataka o radnom danu..." />
      </AppLayout>
    );
  }

  return (
    <AppLayout>
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {error && (
          <div className="mb-4 bg-red-50 border-l-4 border-red-400 p-4">
            <div className="flex">
              <ExclamationCircleIcon className="h-5 w-5 text-red-400 mr-2" />
              <div>
                <p className="text-sm text-red-700">{error}</p>
                <button onClick={loadWorkDay} className="text-sm text-red-600 underline mt-1">
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-900 flex items-center">
            <PowerIcon className="h-8 w-8 mr-3 text-indigo-600" />
            Kraj rada
          </h1>
          <p className="mt-2 text-sm text-gray-600">
            Zatvaranje radnog dana i knjiženje blagajne
          </p>
        </div>

        {/* Status Card */}
        {workDay.isClosed ? (
          <div className="bg-green-100 border border-green-400 rounded-lg p-6 mb-6">
            <div className="flex items-center">
              <CheckCircleIcon className="h-8 w-8 text-green-600 mr-3" />
              <div>
                <h3 className="text-lg font-bold text-green-900">Radni dan je zatvoren</h3>
                <p className="text-sm text-green-700">
                  Radni dan za {new Date(workDay.date).toLocaleDateString('hr-HR')} je uspješno zatvoren
                </p>
              </div>
            </div>
          </div>
        ) : (
          <div className="bg-yellow-100 border border-yellow-400 rounded-lg p-6 mb-6">
            <div className="flex items-center">
              <PowerIcon className="h-8 w-8 text-yellow-600 mr-3" />
              <div>
                <h3 className="text-lg font-bold text-yellow-900">Radni dan je aktivan</h3>
                <p className="text-sm text-yellow-700">
                  Molimo provjerite sve podatke prije zatvaranja radnog dana
                </p>
              </div>
            </div>
          </div>
        )}

        {/* Work Day Summary */}
        <div className="bg-white shadow-md rounded-lg p-6 mb-6">
          <h2 className="text-xl font-bold text-gray-900 mb-4">Pregled radnog dana</h2>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Datum</div>
              <div className="text-lg font-semibold">
                {new Date(workDay.date).toLocaleDateString('hr-HR')}
              </div>
            </div>

            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Blagajnik</div>
              <div className="text-lg font-semibold">{workDay.cashierName}</div>
            </div>

            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Početno stanje blagajne</div>
              <div className="text-lg font-semibold">{workDay.startingCash.toFixed(2)} HRK</div>
            </div>

            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Ukupna prodaja</div>
              <div className="text-lg font-semibold text-green-600">+{workDay.totalSales.toFixed(2)} HRK</div>
            </div>

            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Ukupni rashodi</div>
              <div className="text-lg font-semibold text-red-600">-{workDay.totalExpenses.toFixed(2)} HRK</div>
            </div>

            <div className="border-b pb-4">
              <div className="text-sm text-gray-500">Očekivano stanje</div>
              <div className="text-lg font-semibold">{workDay.expectedCash.toFixed(2)} HRK</div>
            </div>

            <div className="border-b pb-4 md:col-span-2">
              <div className="text-sm text-gray-500">Stvarno stanje u blagajni</div>
              <div className="text-2xl font-bold text-gray-900">{workDay.cashInRegister.toFixed(2)} HRK</div>
            </div>

            <div className="md:col-span-2 bg-gray-50 p-4 rounded-lg">
              <div className="text-sm text-gray-500">Razlika</div>
              <div className={`text-2xl font-bold ${
                workDay.difference === 0 ? 'text-green-600' :
                workDay.difference > 0 ? 'text-blue-600' : 'text-red-600'
              }`}>
                {workDay.difference > 0 ? '+' : ''}{workDay.difference.toFixed(2)} HRK
                {workDay.difference === 0 && ' ✓'}
              </div>
            </div>
          </div>
        </div>

        {/* Actions */}
        <div className="flex justify-end space-x-4">
          <button
            onClick={handlePrintReport}
            className="flex items-center px-6 py-3 bg-gray-600 text-white rounded-md hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
          >
            <PrinterIcon className="h-5 w-5 mr-2" />
            Ispiši izvještaj
          </button>

          {!workDay.isClosed && (
            <button
              onClick={handleCloseDay}
              className="flex items-center px-6 py-3 bg-red-600 text-white rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2"
            >
              <PowerIcon className="h-5 w-5 mr-2" />
              Zatvori radni dan
            </button>
          )}
        </div>
      </div>
    </AppLayout>
  );
};

export default EndOfWorkPage;
