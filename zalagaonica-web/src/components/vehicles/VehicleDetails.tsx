import React, { useState } from 'react';
import { PencilIcon, PlusIcon } from '@heroicons/react/24/solid';
import { VehicleEvent, VehicleEventType } from '../../types/VehicleEventType';
import { Vehicle } from '../../services/vehicleApi';

interface Props {
  vehicle: Vehicle;
  events: VehicleEvent[];
  onEditVehicle: (vehicle: Vehicle) => void;
  onAddEvent: () => void;
}

const eventTypes: VehicleEventType[] = ['Servis', 'Registracija', 'Osiguranje', 'Kazna'];

const VehicleDetails: React.FC<Props> = ({ vehicle, events, onEditVehicle, onAddEvent }) => {
  const [activeTab, setActiveTab] = useState<VehicleEventType>('Servis');

  const filteredEvents = events.filter(e => e.type === activeTab);

  return (
    <div className="bg-white rounded-lg shadow">
      {/* Zaglavlje s osnovnim podacima */}
      <div className="p-4 border-b flex justify-between items-center">
        <div>
            <h2 className="text-xl font-bold">{vehicle.make} {vehicle.model} ({vehicle.year})</h2>
            <p className="text-md text-gray-600">{vehicle.plateNumber || 'N/A'}</p>
        </div>
        <button onClick={() => onEditVehicle(vehicle)} className="p-2 text-gray-500 hover:text-indigo-600">
            <PencilIcon className="h-5 w-5"/>
        </button>
      </div>

      {/* Tabovi */}
      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8 px-4" aria-label="Tabs">
          {eventTypes.map(tab => (
            <button
              key={tab}
              onClick={() => setActiveTab(tab)}
              className={`${activeTab === tab ? 'border-indigo-500 text-indigo-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'} whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm`}
            >
              {tab}
            </button>
          ))}
        </nav>
      </div>

      {/* Sadržaj taba */}
      <div className="p-4">
        <div className="flex justify-between items-center mb-4">
            <h3 className="text-lg font-semibold">Evidencija za: {activeTab}</h3>
            <button onClick={onAddEvent} className="flex items-center bg-indigo-100 text-indigo-700 hover:bg-indigo-200 text-sm font-bold py-1 px-3 rounded-md">
                <PlusIcon className="h-4 w-4 mr-1"/>
                Dodaj
            </button>
        </div>
        
        {/* Tablica s događajima */}
        <div className="overflow-x-auto">
            <table className="min-w-full bg-white text-sm">
                <thead className="bg-gray-50">
                <tr>
                    <th className="py-2 px-3 text-left">Datum</th>
                    <th className="py-2 px-3 text-left">Opis</th>
                    <th className="py-2 px-3 text-left">Status/Istek</th>
                    <th className="py-2 px-3 text-right">Trošak (€)</th>
                </tr>
                </thead>
                <tbody className="divide-y">
                    {filteredEvents.length > 0 ? filteredEvents.map(e => (
                        <tr key={e.id} className="hover:bg-gray-50">
                        <td className="py-2 px-3">{e.date}</td>
                        <td className="py-2 px-3">{e.description}</td>
                        <td className="py-2 px-3">{e.status || e.expiryDate || 'N/A'}</td>
                        <td className="py-2 px-3 text-right font-medium">{e.cost.toFixed(2)}</td>
                        </tr>
                    )) : (
                        <tr>
                            <td colSpan={4} className="text-center py-4 text-gray-500">Nema zapisa za ovu kategoriju.</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
      </div>
    </div>
  );
};

export default VehicleDetails;
