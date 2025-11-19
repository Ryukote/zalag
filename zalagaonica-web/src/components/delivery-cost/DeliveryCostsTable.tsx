import React from 'react';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { DeliveryCost } from '../../types/DeliveryCost';

interface Props {
  data: DeliveryCost[];
  onEdit: (cost: DeliveryCost) => void;
  onDelete: (id: string) => void;
}

export const DeliveryCostsTable: React.FC<Props> = ({ data, onEdit, onDelete }) => {
  return (
    <div className="bg-white shadow-md rounded-lg overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
                <tr>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Datum</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Dostavljač</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Tracking br.</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Opis</th>
                    <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Trošak (€)</th>
                    <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">Akcije</th>
                </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
                {data.map((cost) => (
                    <tr key={cost.id} className="hover:bg-gray-50">
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{cost.date}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{cost.courier}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{cost.trackingNumber}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{cost.description}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-right font-semibold">{cost.cost.toFixed(2)}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                            <button onClick={() => onEdit(cost)} className="text-indigo-600 hover:text-indigo-900 mr-4"><PencilSquareIcon className="h-5 w-5"/></button>
                            <button onClick={() => onDelete(cost.id)} className="text-red-600 hover:text-red-900"><TrashIcon className="h-5 w-5"/></button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
  );
};
