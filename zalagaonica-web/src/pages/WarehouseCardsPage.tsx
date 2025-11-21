import React, { useState, useEffect } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { RectangleStackIcon, PlusIcon, EyeIcon, ExclamationCircleIcon } from '@heroicons/react/24/outline';
import LoadingSpinner from '../components/ui/LoadingSpinner';
import { warehouseCardApi, WarehouseCard } from '../services/warehouseCardApi';

export const WarehouseCardsPage: React.FC = () => {
  const [cards, setCards] = useState<WarehouseCard[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadCards();
  }, []);

  const loadCards = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await warehouseCardApi.getAll();
      setCards(data);
    } catch (err: any) {
      console.error('Error loading warehouse cards:', err);
      setError(err.message || 'Greška pri učitavanju kartica skladišta');
    } finally {
      setLoading(false);
    }
  };

  const getBalanceColor = (balance: number) => {
    if (balance < 10) return 'text-red-600';
    if (balance < 50) return 'text-yellow-600';
    return 'text-green-600';
  };

  if (loading) {
    return (
      <AppLayout>
        <LoadingSpinner fullScreen message="Učitavanje kartica skladišta..." />
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
                <button onClick={loadCards} className="text-sm text-red-600 underline mt-1">
                  Pokušaj ponovno
                </button>
              </div>
            </div>
          </div>
        )}

        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <RectangleStackIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Skladište - Kartice
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Povijest kretanja artikala po skladištu
            </p>
          </div>
          <button className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700">
            <PlusIcon className="h-5 w-5 mr-2" />
            Nova kartica
          </button>
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
              {cards.map((card) => (
                <tr key={card.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(card.date).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-900">
                    <div className="font-medium">{card.articleName}</div>
                    {card.notes && <div className="text-xs text-gray-400">{card.notes}</div>}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    <div>{card.documentType}</div>
                    <div className="text-xs text-gray-400">{card.documentNumber}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-green-600 font-medium">
                    {card.inQuantity > 0 ? `+${card.inQuantity}` : ''}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600 font-medium">
                    {card.outQuantity > 0 ? `-${card.outQuantity}` : ''}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-right">
                    <span className={`font-semibold ${getBalanceColor(card.balance)}`}>
                      {card.balance}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <button className="text-indigo-600 hover:text-indigo-900" title="Pregledaj">
                      <EyeIcon className="h-5 w-5" />
                    </button>
                  </td>
                </tr>
              ))}
              {cards.length === 0 && (
                <tr>
                  <td colSpan={7} className="text-center py-8 text-gray-500">
                    Nema zapisa o karticama skladišta.
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

export default WarehouseCardsPage;
