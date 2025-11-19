import React from 'react';
import { IncomingDocument } from '../../types/incomingDocument';
import { PencilSquareIcon, DocumentTextIcon } from '@heroicons/react/24/outline';

interface DocumentListTableProps {
    documents: IncomingDocument[];
    onEdit: (document: IncomingDocument) => void; // Mijenjamo onRowClick u onEdit
    onGenerateReport?: (document: IncomingDocument) => void;
}

const formatCurrency = (amount: number) => new Intl.NumberFormat('hr-HR', { style: 'currency', currency: 'EUR' }).format(amount);

export const IncomingDocumentListTable: React.FC<DocumentListTableProps> = ({ documents, onEdit, onGenerateReport }) => {
    return (
        <table className="min-w-full divide-y divide-gray-300">
            <thead className="bg-gray-50">
                <tr>
                    <th scope="col" className="py-3.5 pl-6 pr-3 text-left text-sm font-semibold text-gray-900">Dobavljač</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Br. Ulaznog Dokumenta</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Datum Dokumenta</th>
                    <th scope="col" className="px-6 py-3.5 text-right text-sm font-semibold text-gray-900 border-l border-gray-200">Fakturna Vrijednost</th>
                    <th scope="col" className="px-6 py-3.5 text-left text-sm font-semibold text-gray-900 border-l border-gray-200">Skladište</th>
                    <th scope="col" className="relative py-3.5 pl-3 pr-6 border-l border-gray-200"><span className="sr-only">Akcije</span></th>
                </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
                {documents.map((doc) => (
                    <tr key={doc.id} className="hover:bg-gray-50">
                        <td className="whitespace-nowrap py-4 pl-6 pr-3 text-sm font-medium text-gray-900">{doc.supplierName}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 border-l border-gray-200">{doc.documentNumber}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{doc.documentDate}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-700 font-medium text-right border-l border-gray-200">{formatCurrency(doc.invoiceValue)}</td>
                        <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-500 border-l border-gray-200">{doc.warehouseName}</td>
                        <td className="relative whitespace-nowrap py-4 pl-3 pr-6 text-center text-sm font-medium border-l border-gray-200 space-x-3">
                            {onGenerateReport && (
                                <button
                                    onClick={() => onGenerateReport(doc)}
                                    className="text-green-600 hover:text-green-900"
                                    title="Generiraj izvješće"
                                >
                                    <DocumentTextIcon className="h-5 w-5" />
                                </button>
                            )}
                            <button onClick={() => onEdit(doc)} className="text-indigo-600 hover:text-indigo-900"><PencilSquareIcon className="h-5 w-5" /></button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};