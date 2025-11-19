import React from 'react';
import { PencilSquareIcon } from '@heroicons/react/24/outline';
import { CashRegisterTransaction } from '../../types/cashRegisterTransaction';

interface CashRegisterTableProps {
    data: CashRegisterTransaction[];
    onEdit: (transaction: CashRegisterTransaction) => void;
}

const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);
};

export const CashRegisterTable: React.FC<CashRegisterTableProps> = ({ data, onEdit }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">ID</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Datum</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Opis</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">Iznos</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Tip</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Akcije</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.map((transaction) => (
                    <tr key={transaction.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{transaction.id}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{transaction.date}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{transaction.description}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(transaction.amount)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{transaction.type}</td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-right text-sm font-medium border-l border-gray-200">
                            <button onClick={() => onEdit(transaction)} className="text-indigo-600 hover:text-indigo-900">
                                <PencilSquareIcon className="h-5 w-5" />
                                <span className="sr-only">, {transaction.description}</span>
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};
