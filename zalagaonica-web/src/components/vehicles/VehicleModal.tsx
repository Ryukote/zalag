import React, { Fragment, useState, useEffect } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { Vehicle } from '../../services/vehicleApi';

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (vehicle: Vehicle) => void;
  initialData: Vehicle | null;
}

const defaultData: Omit<Vehicle, 'id'> = { make: '', model: '', year: new Date().getFullYear(), plateNumber: '' };

export const VehicleModal: React.FC<Props> = ({ isOpen, onClose, onSave, initialData }) => {
  const [formData, setFormData] = useState(defaultData);

  useEffect(() => {
    setFormData(initialData || defaultData);
  }, [initialData, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({ ...prev, [name]: type === 'number' ? parseInt(value) || 0 : value }));
  };
  
  const handleSave = () => {
    const finalData: Vehicle = {
        id: initialData?.id || crypto.randomUUID(),
        ...formData
    };
    onSave(finalData);
  }

  return (
    <Transition.Root show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={onClose}>
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4">
            <Dialog.Panel className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
              <Dialog.Title as="h3" className="text-xl font-semibold leading-6 text-gray-900">
                {initialData ? 'Uređivanje vozila' : 'Novo vozilo'}
              </Dialog.Title>
              
              <div className="mt-6 grid grid-cols-1 gap-y-6 sm:grid-cols-2 sm:gap-x-4">
                <div>
                  <label htmlFor="make" className="block text-sm font-medium text-gray-900">Marka</label>
                  <input type="text" name="make" id="make" value={formData.make} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                <div>
                  <label htmlFor="model" className="block text-sm font-medium text-gray-900">Model</label>
                  <input type="text" name="model" id="model" value={formData.model} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                <div>
                  <label htmlFor="year" className="block text-sm font-medium text-gray-900">Godište</label>
                  <input type="number" name="year" id="year" value={formData.year} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                </div>
                 <div>
                  <label htmlFor="plateNumber" className="block text-sm font-medium text-gray-900">Registracija</label>
                  <input type="text" name="plateNumber" id="plateNumber" value={formData.plateNumber} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
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
