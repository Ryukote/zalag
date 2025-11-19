import React from 'react';
import { OutputDocument } from '../../types/outputDocument';
import { PencilSquareIcon, TrashIcon, EyeIcon } from '@heroicons/react/24/outline';

interface OutputDocumentListTableProps {
    documents: OutputDocument[];
    onEdit: (document: OutputDocument) => void;
}

const formatCurrency = (amount: number) => new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);

const getStatusColor = (status: string) => {
    switch (status) {
        case 'otvoren':
            return 'bg-yellow-50 text-yellow-800 ring-yellow-600/20';
        case 'plaćen':
            return 'bg-green-50 text-green-700 ring-green-600/20';
        case 'proknjižen':
            return 'bg-indigo-50 text-indigo-700 ring-indigo-600/20';
        default:
            return 'bg-gray-50 text-gray-600 ring-gray-500/10';
    }
};

export const OutputDocumentListTable: React.FC<OutputDocumentListTableProps> = ({ documents, onEdit }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">Br. dok.</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Tip</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Klijent</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Datum</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">Ukupno</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Status</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Akcije</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {documents.map((doc) => (
                    <tr key={doc.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{doc.documentNumber}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{doc.documentType}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{doc.clientName}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{doc.documentDate}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(doc.totalValue)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">
                            <span className={`inline-flex items-center rounded-md px-2 py-1 text-xs font-medium ring-1 ring-inset ${getStatusColor(doc.status)}`}>
                                {doc.status}
                            </span>
                        </td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-center text-sm font-medium border-l border-gray-200">
                            <button onClick={() => onEdit(doc)} className="text-indigo-600 hover:text-indigo-900">
                                <PencilSquareIcon className="h-5 w-5" />
                                <span className="sr-only">, {doc.id}</span>
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};