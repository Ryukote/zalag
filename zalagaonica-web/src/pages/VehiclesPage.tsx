import React, { useState, useMemo, useEffect } from 'react';
import VehicleDetails from '../components/vehicles/VehicleDetails';
import { VehicleEventModal } from '../components/vehicles/VehicleEventModal';
import { VehicleModal } from '../components/vehicles/VehicleModal';
import { VehicleEvent } from '../types/VehicleEventType';
import { Vehicle, vehicleApi } from '../services/vehicleApi';

// Note: Vehicle events are kept as mock data as there's no backend API for them yet
const mockEvents: VehicleEvent[] = [
    { id: 'e1', vehicleId: 'v1', date: '2025-09-15', type: 'Servis', description: 'Redovni servis - izmjena ulja i filtera', cost: 150 },
    { id: 'e2', vehicleId: 'v1', date: '2025-08-20', type: 'Registracija', description: 'Tehnički pregled i produljenje registracije', cost: 200, expiryDate: '2026-08-20' },
    { id: 'e3', vehicleId: 'v1', date: '2025-07-01', type: 'Kazna', description: 'Kazna za nepropisno parkiranje', cost: 50, status: 'plaćeno' },
    { id: 'e4', vehicleId: 'v2', date: '2025-09-01', type: 'Servis', description: 'Zamjena guma', cost: 250 },
    { id: 'e5', vehicleId: 'v2', date: '2025-05-10', type: 'Osiguranje', description: 'Godišnja polica AO', cost: 300, expiryDate: '2026-05-10' },
];

const VehiclesPage: React.FC = () => {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [events, setEvents] = useState<VehicleEvent[]>(mockEvents);
  const [selectedVehicle, setSelectedVehicle] = useState<Vehicle | null>(null);
  const [loading, setLoading] = useState(true);

  const [isVehicleModalOpen, setVehicleModalOpen] = useState(false);
  const [isEventModalOpen, setEventModalOpen] = useState(false);
  const [editingVehicle, setEditingVehicle] = useState<Vehicle | null>(null);

  useEffect(() => {
    loadVehicles();
  }, []);

  const loadVehicles = async () => {
    try {
      setLoading(true);
      const data = await vehicleApi.getAll();
      setVehicles(data);
      if (data.length > 0 && !selectedVehicle) {
        setSelectedVehicle(data[0]);
      }
    } catch (error) {
      console.error('Failed to load vehicles:', error);
      alert('Failed to load vehicles');
    } finally {
      setLoading(false);
    }
  };

  const selectedVehicleEvents = useMemo(() => {
    return selectedVehicle ? events.filter(e => e.vehicleId === selectedVehicle.id) : [];
  }, [selectedVehicle, events]);

  const handleAddNewVehicle = () => {
    setEditingVehicle(null);
    setVehicleModalOpen(true);
  };

  const handleEditVehicle = (vehicle: Vehicle) => {
    setEditingVehicle(vehicle);
    setVehicleModalOpen(true);
  }

  const handleSaveVehicle = async (vehicle: Vehicle) => {
    try {
      if (editingVehicle) {
        await vehicleApi.update(vehicle.id, vehicle);
      } else {
        await vehicleApi.create(vehicle);
      }
      await loadVehicles();
      setVehicleModalOpen(false);
    } catch (error) {
      console.error('Failed to save vehicle:', error);
      alert('Failed to save vehicle');
    }
  }

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-semibold">Evidencija vozila</h1>
        <button onClick={handleAddNewVehicle} className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded-md shadow-sm">
          Dodaj novo vozilo
        </button>
      </div>

      {loading ? (
        <div className="text-center py-4">Loading...</div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          {/* Lista vozila */}
          <div className="lg:col-span-1 bg-white p-4 rounded-lg shadow h-fit">
            <h2 className="text-lg font-bold mb-4">Popis vozila</h2>
            {vehicles.length === 0 ? (
              <p className="text-gray-500">Nema vozila u sustavu.</p>
            ) : (
              <ul className="space-y-2">
                {vehicles.map(v => (
                  <li key={v.id} onClick={() => setSelectedVehicle(v)}
                      className={`p-3 rounded-md cursor-pointer transition-all ${selectedVehicle?.id === v.id ? 'bg-indigo-100 ring-2 ring-indigo-500' : 'hover:bg-gray-100'}`}>
                    <p className="font-semibold">{v.make} {v.model}</p>
                    <p className="text-sm text-gray-600">{v.plateNumber || 'N/A'}</p>
                  </li>
                ))}
              </ul>
            )}
          </div>

          {/* Detalji odabranog vozila */}
          <div className="lg:col-span-3">
            {selectedVehicle ? (
              <VehicleDetails
                vehicle={selectedVehicle}
                events={selectedVehicleEvents}
                onEditVehicle={handleEditVehicle}
                onAddEvent={() => setEventModalOpen(true)}
              />
            ) : (
              <div className="flex items-center justify-center h-full bg-white p-4 rounded-lg shadow">
                <p className="text-gray-500">Odaberite vozilo za prikaz detalja.</p>
              </div>
            )}
          </div>
        </div>
      )}

      {isVehicleModalOpen && (
        <VehicleModal
            isOpen={isVehicleModalOpen}
            initialData={editingVehicle}
            onClose={() => setVehicleModalOpen(false)}
            onSave={handleSaveVehicle}
        />
      )}

      {isEventModalOpen && selectedVehicle && (
        <VehicleEventModal
            isOpen={isEventModalOpen}
            vehicleId={selectedVehicle.id}
            onClose={() => setEventModalOpen(false)}
            onSave={(event: any) => { /* logika za spremanje */ setEventModalOpen(false); }}
        />
      )}
    </div>
  );
};

export default VehiclesPage;
