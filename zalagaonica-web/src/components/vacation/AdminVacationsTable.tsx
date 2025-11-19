import React, { useState } from 'react';
import { CheckCircleIcon, XCircleIcon, PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { Vacation, VacationStatus, getVacationStatusText, getVacationTypeText } from '../../types/HR';
import { ConfirmDialog } from '../ui/ConfirmDialog';

interface Props {
  data: Vacation[];
  onEdit: (vacation: Vacation) => void;
  onDelete: (id: string) => void;
  onApprove: (id: string) => void;
  onReject: (id: string) => void;
}

const getStatusClass = (status: VacationStatus) => {
  switch (status) {
    case VacationStatus.Approved: return 'bg-green-100 text-green-800';
    case VacationStatus.Pending: return 'bg-yellow-100 text-yellow-800';
    case VacationStatus.Rejected: return 'bg-red-100 text-red-800';
    default: return 'bg-gray-100 text-gray-800';
  }
};

export const AdminVacationsTable: React.FC<Props> = ({
  data,
  onEdit,
  onDelete,
  onApprove,
  onReject
}) => {
  const [confirmDialog, setConfirmDialog] = useState<{
    isOpen: boolean;
    title: string;
    message: string;
    onConfirm: () => void;
    type: 'approve' | 'reject' | 'delete';
  }>({
    isOpen: false,
    title: '',
    message: '',
    onConfirm: () => {},
    type: 'approve'
  });

  const handleApprove = (vacation: Vacation) => {
    setConfirmDialog({
      isOpen: true,
      title: 'Odobri zahtjev',
      message: `Jeste li sigurni da želite odobriti zahtjev za ${vacation.employeeName} (${vacation.startDate} - ${vacation.endDate})?`,
      onConfirm: () => {
        onApprove(vacation.id);
        setConfirmDialog(prev => ({ ...prev, isOpen: false }));
      },
      type: 'approve'
    });
  };

  const handleReject = (vacation: Vacation) => {
    setConfirmDialog({
      isOpen: true,
      title: 'Odbij zahtjev',
      message: `Jeste li sigurni da želite odbiti zahtjev za ${vacation.employeeName} (${vacation.startDate} - ${vacation.endDate})?`,
      onConfirm: () => {
        onReject(vacation.id);
        setConfirmDialog(prev => ({ ...prev, isOpen: false }));
      },
      type: 'reject'
    });
  };

  const handleDelete = (vacation: Vacation) => {
    setConfirmDialog({
      isOpen: true,
      title: 'Obriši zahtjev',
      message: `Jeste li sigurni da želite obrisati zahtjev za ${vacation.employeeName}?`,
      onConfirm: () => {
        onDelete(vacation.id);
        setConfirmDialog(prev => ({ ...prev, isOpen: false }));
      },
      type: 'delete'
    });
  };

  return (
    <>
      <div className="bg-white shadow-md rounded-lg overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Zaposlenik</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tip</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Početak</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Završetak</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
              <th className="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Akcije</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {data.map((vac) => (
              <tr key={vac.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{vac.employeeName}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm capitalize">{getVacationTypeText(vac.type)}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm">{new Date(vac.startDate).toLocaleDateString('hr-HR')}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm">{new Date(vac.endDate).toLocaleDateString('hr-HR')}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm">
                  <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getStatusClass(vac.status)}`}>
                    {getVacationStatusText(vac.status)}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                  <div className="flex justify-center items-center space-x-2">
                    {vac.status === VacationStatus.Pending && (
                      <>
                        <button
                          onClick={() => handleApprove(vac)}
                          className="text-green-600 hover:text-green-900"
                          title="Odobri"
                        >
                          <CheckCircleIcon className="h-5 w-5" />
                        </button>
                        <button
                          onClick={() => handleReject(vac)}
                          className="text-red-600 hover:text-red-900"
                          title="Odbij"
                        >
                          <XCircleIcon className="h-5 w-5" />
                        </button>
                      </>
                    )}
                    <button
                      onClick={() => onEdit(vac)}
                      className="text-indigo-600 hover:text-indigo-900"
                      title="Uredi"
                    >
                      <PencilSquareIcon className="h-5 w-5" />
                    </button>
                    <button
                      onClick={() => handleDelete(vac)}
                      className="text-gray-600 hover:text-gray-900"
                      title="Obriši"
                    >
                      <TrashIcon className="h-5 w-5" />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
            {data.length === 0 && (
              <tr>
                <td colSpan={6} className="text-center py-8 text-gray-500">
                  Nema zapisa o zahtjevima za godišnji odmor.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      <ConfirmDialog
        isOpen={confirmDialog.isOpen}
        title={confirmDialog.title}
        message={confirmDialog.message}
        onConfirm={confirmDialog.onConfirm}
        onClose={() => setConfirmDialog(prev => ({ ...prev, isOpen: false }))}
        confirmText={confirmDialog.type === 'approve' ? 'Odobri' : confirmDialog.type === 'reject' ? 'Odbij' : 'Obriši'}
        cancelText="Odustani"
      />
    </>
  );
};
