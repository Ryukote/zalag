import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { Payroll, Employee } from '../../types/HR';

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (payroll: Payroll) => void;
  initialData: Payroll | null;
  employees: Employee[];
}

const defaultData: Omit<Payroll, 'id' | 'employeeName'> = {
    employeeId: '',
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear(),
    grossSalary: 0,
    netSalary: 0,
    tax: 0,
    socialContributions: 0,
    paid: false
};

export const PayrollModal: React.FC<Props> = ({ isOpen, onClose, onSave, initialData, employees }) => {
  const [formData, setFormData] = useState(defaultData);

  useEffect(() => {
    const dataToSet = initialData ? { ...initialData } : { ...defaultData, employeeId: employees[0]?.id || '' };
    delete (dataToSet as Partial<Payroll>).employeeName;
    setFormData(dataToSet);
  }, [initialData, isOpen, employees]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({ ...prev, [name]: type === 'number' ? parseFloat(value) || 0 : value }));
  };
  
  const handleSave = () => {
    if (!formData.employeeId) {
        alert("Molimo odaberite zaposlenika.");
        return;
    }
    const employeeName = employees.find(e => e.id === formData.employeeId)?.fullName;
    const finalData: Payroll = {
        id: initialData?.id || crypto.randomUUID(),
        employeeName: employeeName,
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
                {initialData ? 'Uredi obračun plaće' : 'Novi obračun plaće'}
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
                  <label htmlFor="month" className="block text-sm font-medium">Mjesec (1-12)</label>
                  <input type="number" name="month" id="month" min="1" max="12" value={formData.month} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                <div>
                  <label htmlFor="year" className="block text-sm font-medium">Godina</label>
                  <input type="number" name="year" id="year" value={formData.year} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                 <div>
                  <label htmlFor="grossSalary" className="block text-sm font-medium">Bruto plaća (€)</label>
                  <input type="number" name="grossSalary" id="grossSalary" value={formData.grossSalary} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                 <div>
                  <label htmlFor="netSalary" className="block text-sm font-medium">Neto plaća (€)</label>
                  <input type="number" name="netSalary" id="netSalary" value={formData.netSalary} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                 <div>
                  <label htmlFor="tax" className="block text-sm font-medium">Porez (€)</label>
                  <input type="number" name="tax" id="tax" value={formData.tax} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                 <div>
                  <label htmlFor="socialContributions" className="block text-sm font-medium">Doprinosi (€)</label>
                  <input type="number" name="socialContributions" id="socialContributions" value={formData.socialContributions} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
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

