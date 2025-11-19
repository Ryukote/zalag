import React from 'react';
import { PriceListItem } from '../../types/priceListItem';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

interface PriceListTableProps {
    data: PriceListItem[];
    onEdit: (item: PriceListItem) => void;
}

const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);
};

export const PriceListTable: React.FC<PriceListTableProps> = ({ data, onEdit }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">Oznaka</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Naziv</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Stanje</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">MPC</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">MPCP</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Grupa</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Porez</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Rbr. Tarife</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Dobavljač</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Uredi</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.map((item) => (
                    <tr key={item.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{item.id}</td>
                        <td className="px-6 py-4 text-sm font-semibold text-gray-800 border-l border-gray-200">{item.name}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{`${item.stock} ${item.unitOfMeasure}`}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(item.retailPrice)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(item.retailPriceWithTax)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{item.group}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{`${item.taxRate}%`}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{item.taxTariffNumber}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{item.supplierName}</td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-right text-sm font-medium border-l border-gray-200">
                            <button onClick={() => onEdit(item)} className="text-indigo-600 hover:text-indigo-900"><PencilSquareIcon className="h-5 w-5" /></button>
                            <button className="ml-4 text-red-600 hover:text-red-900"><TrashIcon className="h-5 w-5" /></button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};