import React from 'react';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { UnitOfMeasure } from '../../types/unitsOfMeasure';

interface UnitOfMeasureTableProps {
    data: UnitOfMeasure[];
    onEdit: (item: UnitOfMeasure) => void;
    onDelete: (id: string) => void;
}

export const UnitOfMeasureTable: React.FC<UnitOfMeasureTableProps> = ({ data, onEdit, onDelete }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">ID</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Naziv</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Šifra</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Akcije</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {data.length > 0 ? (
                    data.map((item) => (
                        <tr key={item.id} className="hover:bg-gray-50">
                            <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{item.id}</td>
                            <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{item.name}</td>
                            <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{item.code}</td>
                            <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-right text-sm font-medium border-l border-gray-200">
                                <button onClick={() => onEdit(item)} className="text-indigo-600 hover:text-indigo-900">
                                    <PencilSquareIcon className="h-5 w-5" />
                                    <span className="sr-only">, {item.name}</span>
                                </button>
                                <button onClick={() => onDelete(item.id)} className="ml-4 text-red-600 hover:text-red-900">
                                    <TrashIcon className="h-5 w-5" />
                                    <span className="sr-only">, {item.name}</span>
                                </button>
                            </td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td colSpan={4} className="py-4 text-center text-sm text-gray-500">Nema pronađenih jedinica mjere.</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};