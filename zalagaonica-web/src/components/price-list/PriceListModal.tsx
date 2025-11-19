import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { PriceListItem } from '../../types/priceListItem';

interface PriceListModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (item: PriceListItem) => void;
  initialData: PriceListItem | null;
}

const defaultItem: PriceListItem = { id: '', name: '', stock: 0, unitOfMeasure: 'kom', retailPrice: 0, retailPriceWithTax: 0, taxRate: 25, supplierName: '', group: '', taxTariffNumber: '' };

export const PriceListModal: React.FC<PriceListModalProps> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState<PriceListItem>(defaultItem);

  useEffect(() => {
    setFormData(initialData || defaultItem);
  }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const isNumber = e.target.type === 'number';
    setFormData(prev => ({ ...prev, [name]: isNumber ? parseFloat(value) || 0 : value }));
  };
  
  const handleSave = () => {
    onSave(formData);
  };

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <Transition.Child as={Fragment} enter="ease-out duration-300" enterFrom="opacity-0" enterTo="opacity-100" leave="ease-in duration-200" leaveFrom="opacity-100" leaveTo="opacity-0">
          <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        </Transition.Child>
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-3xl rounded-lg bg-white p-6 shadow-xl">
              <Dialog.Title as="h3" className="text-xl font-semibold leading-6 text-gray-900">
                {initialData ? 'Uređivanje stavke' : 'Nova stavka cjenika'}
              </Dialog.Title>
              
              {/* OVDJE SU SADA SVA POTREBNA POLJA */}
              <div className="mt-6 grid grid-cols-1 gap-y-6 sm:grid-cols-3 sm:gap-x-4">
                <div>
                  <label htmlFor="name" className="block text-sm font-medium leading-6 text-gray-900">Naziv</label>
                  <input type="text" name="name" id="name" value={formData.name} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="id" className="block text-sm font-medium leading-6 text-gray-900">Oznaka (ID)</label>
                  <input type="text" name="id" id="id" value={formData.id} onChange={handleChange} disabled={!!initialData} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 disabled:bg-gray-50 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="stock" className="block text-sm font-medium leading-6 text-gray-900">Stanje</label>
                  <input type="number" name="stock" id="stock" value={formData.stock} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="unitOfMeasure" className="block text-sm font-medium leading-6 text-gray-900">Jedinica mjere</label>
                  <input type="text" name="unitOfMeasure" id="unitOfMeasure" value={formData.unitOfMeasure} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="retailPrice" className="block text-sm font-medium leading-6 text-gray-900">MPC (€)</label>
                  <input type="number" name="retailPrice" id="retailPrice" value={formData.retailPrice} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                 <div>
                  <label htmlFor="retailPriceWithTax" className="block text-sm font-medium leading-6 text-gray-900">MPCP (€)</label>
                  <input type="number" name="retailPriceWithTax" id="retailPriceWithTax" value={formData.retailPriceWithTax} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="group" className="block text-sm font-medium leading-6 text-gray-900">Grupa</label>
                  <input type="text" name="group" id="group" value={formData.group || ''} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="taxRate" className="block text-sm font-medium leading-6 text-gray-900">Porez (%)</label>
                  <input type="number" name="taxRate" id="taxRate" value={formData.taxRate} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                <div>
                  <label htmlFor="taxTariffNumber" className="block text-sm font-medium leading-6 text-gray-900">Rbr. Tarife</label>
                  <input type="text" name="taxTariffNumber" id="taxTariffNumber" value={formData.taxTariffNumber || ''} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
                 <div>
                  <label htmlFor="supplierName" className="block text-sm font-medium leading-6 text-gray-900">Dobavljač</label>
                  <input type="text" name="supplierName" id="supplierName" value={formData.supplierName} onChange={handleChange} className="mt-1 block w-full rounded-md border-0 py-1.5 px-3 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm"/>
                </div>
              </div>

              <div className="mt-8 flex justify-end space-x-3">
                <button type="button" onClick={onClose} className="rounded-md bg-white px-4 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50">Odustani</button>
                <button type="button" onClick={handleSave} className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-700">Spremi</button>
              </div>
            </Dialog.Panel>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
};