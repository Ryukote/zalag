import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { Vacation, Employee, VacationStatus, VacationType, getVacationStatusText, getVacationTypeText } from '../../types/HR';

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (vacation: Vacation) => void;
  initialData: Vacation | null;
  employees: Employee[];
}

const defaultData: Omit<Vacation, 'id' | 'employeeName' | 'requestDate' | 'totalDays'> = {
    employeeId: '',
    startDate: new Date().toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0],
    status: VacationStatus.Pending,
    type: VacationType.AnnualLeave,
    reason: ''
};

export const VacationModal: React.FC<Props> = ({ isOpen, onClose, onSave, initialData, employees }) => {
  const [formData, setFormData] = useState(defaultData);

  useEffect(() => {
    if (initialData) {
      const { employeeName, requestDate, totalDays, ...rest } = initialData;
      setFormData(rest);
    } else {
      setFormData({ ...defaultData, employeeId: employees[0]?.id || '' });
    }
  }, [initialData, isOpen, employees]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    if (name === 'status' || name === 'type') {
      setFormData(prev => ({ ...prev, [name]: parseInt(value) }));
    } else {
      setFormData(prev => ({ ...prev, [name]: value }));
    }
  };
  
  const handleSave = () => {
    if (!formData.employeeId) {
        alert("Molimo odaberite zaposlenika.");
        return;
    }
    const employee = employees.find(e => e.id === formData.employeeId);
    const finalData: Vacation = {
        id: initialData?.id || crypto.randomUUID(),
        employeeName: employee?.fullName || '',
        requestDate: initialData?.requestDate || new Date().toISOString(),
        totalDays: Math.ceil((new Date(formData.endDate).getTime() - new Date(formData.startDate).getTime()) / (1000 * 60 * 60 * 24)) + 1,
        ...formData
    };
    onSave(finalData);
  }

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
              <Dialog.Title as="h3" className="text-xl font-semibold">
                {initialData ? 'Uredi zahtjev' : 'Novi zahtjev za odsutnost'}
              </Dialog.Title>
              
              <div className="mt-6 grid grid-cols-1 gap-y-6 sm:grid-cols-2 sm:gap-x-4">
                <div className="sm:col-span-2">
                  <label htmlFor="employeeId" className="block text-sm font-medium">Zaposlenik</label>
                  <select name="employeeId" id="employeeId" value={formData.employeeId} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:ring-indigo-500 focus:border-indigo-500">
                      <option value="" disabled>Odaberite zaposlenika</option>
                      {employees.map(emp => <option key={emp.id} value={emp.id}>{emp.fullName}</option>)}
                  </select>
                </div>
                <div>
                  <label htmlFor="startDate" className="block text-sm font-medium">Početni datum</label>
                  <input type="date" name="startDate" id="startDate" value={formData.startDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                <div>
                  <label htmlFor="endDate" className="block text-sm font-medium">Završni datum</label>
                  <input type="date" name="endDate" id="endDate" value={formData.endDate} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                <div>
                  <label htmlFor="type" className="block text-sm font-medium">Tip odsutnosti</label>
                  <select name="type" id="type" value={formData.type} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                      <option value={VacationType.AnnualLeave}>Godišnji odmor</option>
                      <option value={VacationType.SickLeave}>Bolovanje</option>
                      <option value={VacationType.PaidLeave}>Plaćeni dopust</option>
                  </select>
                </div>
                <div>
                  <label htmlFor="status" className="block text-sm font-medium">Status</label>
                  <select name="status" id="status" value={formData.status} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                      <option value={VacationStatus.Pending}>Na čekanju</option>
                      <option value={VacationStatus.Approved}>Odobreno</option>
                      <option value={VacationStatus.Rejected}>Odbijeno</option>
                  </select>
                </div>
                <div className="sm:col-span-2">
                  <label htmlFor="reason" className="block text-sm font-medium">Razlog (opcionalno)</label>
                  <textarea name="reason" id="reason" value={formData.reason || ''} onChange={handleChange} rows={3} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm" placeholder="Unesite razlog..."></textarea>
                </div>
              </div>

              <div className="mt-8 flex justify-end space-x-3">
                <button type="button" onClick={onClose} className="rounded-md bg-white px-4 py-2 text-sm font-semibold ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
                <button type="button" onClick={handleSave} className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi</button>
              </div>
            </Dialog.Panel>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};

