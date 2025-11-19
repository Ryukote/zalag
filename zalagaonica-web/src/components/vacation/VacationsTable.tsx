import React from 'react';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { Vacation, VacationStatus } from '../../types/HR';

interface Props {
  data: Vacation[];
  onEdit: (vacation: Vacation) => void;
  onDelete: (id: string) => void;
}

const getStatusClass = (status: Vacation['status']) => {
  switch (status) {
    case VacationStatus.Approved: return 'bg-green-100 text-green-800';
    case VacationStatus.Pending: return 'bg-yellow-100 text-yellow-800';
    case VacationStatus.Rejected: return 'bg-red-100 text-red-800';
    default: return 'bg-gray-100 text-gray-800';
  }
};

export const VacationsTable: React.FC<Props> = ({ data, onEdit, onDelete }) => {
  return (
    <div className="bg-white shadow-md rounded-lg overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
                <tr>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Zaposlenik</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Tip</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Početak</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Završetak</th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                    <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">Akcije</th>
                </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
                {data.map((vac) => (
                    <tr key={vac.id} className="hover:bg-gray-50">
                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{vac.employeeName}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm capitalize">{vac.type}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{vac.startDate}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{vac.endDate}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">
                            <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusClass(vac.status)}`}>
                                {vac.status}
                            </span>
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                            <button onClick={() => onEdit(vac)} className="text-indigo-600 hover:text-indigo-900 mr-4"><PencilSquareIcon className="h-5 w-5"/></button>
                            <button onClick={() => onDelete(vac.id)} className="text-red-600 hover:text-red-900"><TrashIcon className="h-5 w-5"/></button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
  );
};

