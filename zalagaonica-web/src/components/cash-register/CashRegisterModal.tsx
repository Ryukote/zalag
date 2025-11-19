import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { CashRegisterTransaction } from '../../types/cashRegisterTransaction';

interface CashRegisterModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (transaction: CashRegisterTransaction) => void;
  initialData: CashRegisterTransaction | null;
}

const defaultTransaction: CashRegisterTransaction = {
  id: '',
  date: new Date().toISOString().split('T')[0],
  description: '',
  amount: 0,
  type: 'Uplata',
};

export const CashRegisterModal: React.FC<CashRegisterModalProps> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState<CashRegisterTransaction>(defaultTransaction);

  useEffect(() => {
    setFormData(initialData || { ...defaultTransaction, id: String(Date.now()) });
  }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: e.target.type === 'number' ? parseFloat(value) || 0 : value,
    }));
  };
  
  const handleSave = () => {
    onSave(formData);
    onClose();
  };

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
              enterTo="opacity-100 translate-y-0 sm:scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 translate-y-0 sm:scale-100"
              leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            >
              <Dialog.Panel className="relative transform overflow-hidden rounded-lg bg-white px-4 pb-4 pt-5 text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg sm:p-6">
                <div>
                  <div className="mt-3 text-center sm:mt-0 sm:text-left">
                    <Dialog.Title as="h3" className="text-xl font-semibold leading-6 text-gray-900 mb-6">
                      {initialData ? 'Uredi transakciju' : 'Nova transakcija'}
                    </Dialog.Title>
                    <div className="mt-2 space-y-4">
                      {/* Polja obrasca */}
                      <div>
                        <label htmlFor="date" className="block text-sm font-medium leading-6 text-gray-900">Datum</label>
                        <input type="date" name="date" id="date" value={formData.date} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm" />
                      </div>
                      <div>
                        <label htmlFor="description" className="block text-sm font-medium leading-6 text-gray-900">Opis</label>
                        <input type="text" name="description" id="description" value={formData.description} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm" />
                      </div>
                      <div>
                        <label htmlFor="amount" className="block text-sm font-medium leading-6 text-gray-900">Iznos</label>
                        <input type="number" name="amount" id="amount" value={formData.amount} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm" />
                      </div>
                      <div>
                        <label htmlFor="type" className="block text-sm font-medium leading-6 text-gray-900">Tip transakcije</label>
                        <select name="type" id="type" value={formData.type} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm">
                          <option>Uplata</option>
                          <option>Isplata</option>
                        </select>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="mt-5 sm:mt-6 sm:flex sm:flex-row-reverse">
                  <button
                    type="button"
                    className="inline-flex w-full justify-center rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 sm:ml-3 sm:w-auto"
                    onClick={handleSave}
                  >
                    Spremi
                  </button>
                  <button
                    type="button"
                    className="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0 sm:w-auto"
                    onClick={onClose}
                  >
                    Odustani
                  </button>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};
