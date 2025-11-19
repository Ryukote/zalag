import React, { useState } from 'react';
import { AppLayout } from '../components/layout/AppLayout';
import { RectangleStackIcon, PlusIcon, EyeIcon, ArrowUpOnSquareIcon, ArrowDownOnSquareIcon } from '@heroicons/react/24/outline';

interface WarehouseCard {
  id: string;
  articleCode: string;
  articleName: string;
  warehouse: string;
  currentStock: number;
  reservedStock: number;
  availableStock: number;
  unitOfMeasure: string;
  lastMovement: string;
}

export const WarehouseCardsPage: React.FC = () => {
  const [cards, setCards] = useState<WarehouseCard[]>([
    {
      id: '1',
      articleCode: 'ART-001',
      articleName: 'Zlatna narukvica 14K',
      warehouse: 'Zalagaonica (ZG3)',
      currentStock: 25,
      reservedStock: 3,
      availableStock: 22,
      unitOfMeasure: 'kom',
      lastMovement: '2025-01-15'
    },
    {
      id: '2',
      articleCode: 'ART-002',
      articleName: 'Srebrna ogrlica',
      warehouse: 'Zalagaonica (ZG3)',
      currentStock: 50,
      reservedStock: 5,
      availableStock: 45,
      unitOfMeasure: 'kom',
      lastMovement: '2025-01-14'
    }
  ]);

  const getStockColor = (available: number, total: number) => {
    const percentage = (available / total) * 100;
    if (percentage < 20) return 'text-red-600';
    if (percentage < 50) return 'text-yellow-600';
    return 'text-green-600';
  };

  return (
    <AppLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center">
              <RectangleStackIcon className="h-8 w-8 mr-3 text-indigo-600" />
              Skladište - Kartice
            </h1>
            <p className="mt-2 text-sm text-gray-600">
              Pregled kartica skladišta po artiklima
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
                  Šifra artikla
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Naziv artikla
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Skladište
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Stanje
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Rezervirano
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Raspoloživo
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Zadnje kretanje
                </th>
                <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Akcije
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {cards.map((card) => (
                <tr key={card.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {card.articleCode}
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {card.articleName}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {card.warehouse}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">
                    {card.currentStock} {card.unitOfMeasure}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {card.reservedStock} {card.unitOfMeasure}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <span className={`font-semibold ${getStockColor(card.availableStock, card.currentStock)}`}>
                      {card.availableStock} {card.unitOfMeasure}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(card.lastMovement).toLocaleDateString('hr-HR')}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center items-center space-x-2">
                      <button className="text-indigo-600 hover:text-indigo-900" title="Pregledaj karticu">
                        <EyeIcon className="h-5 w-5" />
                      </button>
                      <button className="text-green-600 hover:text-green-900" title="Ulaz">
                        <ArrowDownOnSquareIcon className="h-5 w-5" />
                      </button>
                      <button className="text-red-600 hover:text-red-900" title="Izlaz">
                        <ArrowUpOnSquareIcon className="h-5 w-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {cards.length === 0 && (
                <tr>
                  <td colSpan={8} className="text-center py-8 text-gray-500">
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
