import React, { Fragment, useState } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { VehicleEvent, VehicleEventType } from '../../types/VehicleEventType';

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onSave: (event: VehicleEvent) => void;
  vehicleId: string;
}

const eventTypes: VehicleEventType[] = ['Servis', 'Registracija', 'Osiguranje', 'Kazna'];

export const VehicleEventModal: React.FC<Props> = ({ isOpen, onClose, onSave, vehicleId }) => {
  const [formData, setFormData] = useState<Omit<VehicleEvent, 'id' | 'vehicleId'>>({
      date: new Date().toISOString().split('T')[0],
      type: 'Servis',
      description: '',
      cost: 0
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({ ...prev, [name]: type === 'number' ? parseFloat(value) || 0 : value }));
  };

  const handleSave = () => {
    const finalData: VehicleEvent = {
        id: crypto.randomUUID(),
        vehicleId: vehicleId,
        ...formData
    }
    onSave(finalData);
  }

  return (
    <Transition.Root show={isOpen} as={Fragment}>
        <Dialog as="div" className="relative z-10" onClose={onClose}>
            <div className="fixed inset-0 bg-gray-500 bg-opacity-75" />
            <div className="fixed inset-0 z-10 overflow-y-auto">
                <div className="flex min-h-full items-center justify-center p-4">
                    <Dialog.Panel className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
                        <Dialog.Title as="h3" className="text-xl font-semibold">Novi unos</Dialog.Title>
                        
                        <div className="mt-6 grid grid-cols-1 gap-y-6 sm:grid-cols-2 sm:gap-x-4">
                            <div>
                                <label htmlFor="type" className="block text-sm font-medium">Tip unosa</label>
                                <select name="type" id="type" value={formData.type} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                                    {eventTypes.map(type => <option key={type}>{type}</option>)}
                                </select>
                            </div>
                            <div>
                                <label htmlFor="date" className="block text-sm font-medium">Datum</label>
                                <input type="date" name="date" id="date" value={formData.date} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                            </div>
                            <div className="sm:col-span-2">
                                <label htmlFor="description" className="block text-sm font-medium">Opis</label>
                                <textarea name="description" id="description" value={formData.description} onChange={handleChange} rows={3} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                            </div>
                             <div>
                                <label htmlFor="cost" className="block text-sm font-medium">Trošak (€)</label>
                                <input type="number" name="cost" id="cost" value={formData.cost} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                            </div>

                            {(formData.type === 'Registracija' || formData.type === 'Osiguranje') && (
                                <div>
                                    <label htmlFor="expiryDate" className="block text-sm font-medium">Datum isteka</label>
                                    <input type="date" name="expiryDate" id="expiryDate" value={formData.expiryDate || ''} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"/>
                                </div>
                            )}

                             {formData.type === 'Kazna' && (
                                <div>
                                    <label htmlFor="status" className="block text-sm font-medium">Status</label>
                                    <select name="status" id="status" value={formData.status || 'neplaćeno'} onChange={handleChange} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                                        <option>neplaćeno</option>
                                        <option>plaćeno</option>
                                    </select>
                                </div>
                            )}
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
