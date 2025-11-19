import React from 'react';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { Payroll } from '../../types/HR';

interface Props {
  data: Payroll[];
  onEdit: (payroll: Payroll) => void;
  onDelete: (id: string) => void;
}

export const PayrollTable: React.FC<Props> = ({ data, onEdit, onDelete }) => {
  return (
    <div className="bg-white shadow-md rounded-lg overflow-x-auto">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Zaposlenik</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Period</th>
            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Bruto (€)</th>
            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Neto (€)</th>
            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Porez (€)</th>
            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Doprinosi (€)</th>
            <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">Status</th>
            <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase">Akcije</th>
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {data.map((p) => (
            <tr key={p.id} className="hover:bg-gray-50">
              <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{p.employeeName}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm">{p.month}/{p.year}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-right">{p.grossSalary.toFixed(2)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-right font-bold text-green-600">{p.netSalary.toFixed(2)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-red-600">{p.tax.toFixed(2)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-right text-orange-600">{p.socialContributions.toFixed(2)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-center text-sm">
                <span className={`px-2 py-1 rounded-full text-xs ${p.paid ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>
                  {p.paid ? 'Plaćeno' : 'Neplaćeno'}
                </span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                <button onClick={() => onEdit(p)} className="text-indigo-600 hover:text-indigo-900 mr-4"><PencilSquareIcon className="h-5 w-5"/></button>
                <button onClick={() => onDelete(p.id)} className="text-red-600 hover:text-red-900"><TrashIcon className="h-5 w-5"/></button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

